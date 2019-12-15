using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.Models;
using EventManager.API.ResourceParameters;
using EventManager.API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public EventsController (
            IEventRepository eventRepository,
            IMapper mapper,
            IPropertyCheckerService propertyCheckerService,
            IPropertyMappingService propertyMappingService)
        {
            _eventRepository = eventRepository ??
                throw new ArgumentNullException (nameof (eventRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException (nameof (propertyMappingService));
            _propertyCheckerService = propertyCheckerService ??
                throw new ArgumentNullException (nameof (propertyCheckerService));
        }

        [HttpGet (Name = "GetEvents")]
        public IActionResult GetEvents ([FromQuery] EventsResourceParameters eventsResourceParameters, [FromHeader (Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse (mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest (new
                {
                    message = "Accept header mediaType is not allowed"
                });
            }

            if (!_propertyMappingService.ValidMappingExistsFor<EventDto, Event> (eventsResourceParameters.OrderBy))
            {
                return BadRequest ();
            }

            if (!_propertyCheckerService.TypeHasProperties<EventDto> (eventsResourceParameters.Fields))
            {
                return BadRequest ();
            }

            var events = _eventRepository.GetEvents (eventsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = events.TotalCount,
                pageSize = events.PageSize,
                currentPage = events.CurrentPage,
                totalPages = events.TotalPages
            };

            Response.Headers.Add ("X-Pagination", JsonSerializer.Serialize (paginationMetadata));

            var links = CreateLinksForEvents (eventsResourceParameters, events.HasNext, events.HasPrevious);
            var shapeEvents = _mapper.Map<IEnumerable<EventDto>> (events).ShapeData (eventsResourceParameters.Fields);

            if (parsedMediaType.MediaType == "application/vnd.marvin.hateoas+json")
            {
                var shapeEventsWithLinks = shapeEvents.Select (evt =>
                {
                    var eventAsDictionary = evt as IDictionary<string, object>;
                    var eventLinks = CreateLinksForEvent ((Guid) eventAsDictionary["Id"], null);

                    eventAsDictionary.Add ("links", eventLinks);

                    return eventAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapeEventsWithLinks,
                    links,
                };

                return Ok (linkedCollectionResource);
            }

            return Ok (shapeEvents);
        }

        [HttpGet ("{eventId}", Name = "GetEventById")]
        public async Task<IActionResult> GetEventById (Guid eventId, string fields, [FromHeader (Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse (mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest ();
            }

            if (string.IsNullOrWhiteSpace (eventId.ToString ()))
            {
                return BadRequest (new
                {
                    message = "Event Id should not be null or empty!"
                });
            }

            if (!_propertyCheckerService.TypeHasProperties<CenterDto> (fields))
            {
                return BadRequest ();
            }

            var eventEntity = await _eventRepository.GetEventByIdAsync (eventId);

            if (eventEntity == null)
            {
                return NotFound ();
            }

            if (parsedMediaType.MediaType == "application/vnd.marvin.hateoas+json")
            {
                var links = CreateLinksForEvent (eventId, fields);

                var linkedResourceToReturn = _mapper.Map<EventDto> (eventEntity).ShapeData (fields) as IDictionary<string, object>;

                linkedResourceToReturn.Add ("links", links);

                return Ok (linkedResourceToReturn);
            }

            return Ok (_mapper.Map<EventDto> (eventEntity).ShapeData (fields));
        }

        [HttpPut ("{eventId}")]
        public async Task<IActionResult> UpdateEvent (Guid centerId, Guid eventId, [FromBody] EventForUpdateDto eventForUpdate)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync (eventId);

            if (eventEntity == null)
            {
                return NotFound ();
            }

            if (DateTimeOffset.TryParse (eventForUpdate.ScheduledDate, out var parsedDate))
            {
                if (parsedDate.Date <= DateTimeOffset.Now.Date)
                {
                    return BadRequest (new
                    {
                        message = "Event can't be scheduled on or before this day"
                    });
                }

                var eventExist = await _eventRepository.CheckIfEventExistForCenterAsync (centerId, eventId, parsedDate);

                if (eventExist)
                {
                    return Conflict (new
                    {
                        message = $"An existing event was already scheduled for this center on this day."
                    });
                }
            }
            else
            {
                return BadRequest (new
                {
                    message = "Event date is not in correct format"
                });
            }

            _mapper.Map (eventForUpdate, eventEntity);

            _eventRepository.UpdateEvent (eventEntity);
            await _eventRepository.SaveChangesAsync ();

            return NoContent ();
        }

        [HttpDelete ("{eventId}", Name = "DeleteEvent")]
        public async Task<IActionResult> DeleteEvent (Guid eventId)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync (eventId);

            if (eventEntity == null)
            {
                return NotFound ();
            }

            _eventRepository.DeleteEvent (eventEntity);
            await _eventRepository.SaveChangesAsync ();

            return NoContent ();
        }

        private string CreateEventResourceUri (EventsResourceParameters eventsResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
            case ResourceUriType.PreviousPage:
                return Url.Link ("GetEvents", new
                {
                    fields = eventsResourceParameters.Fields,
                        orderBy = eventsResourceParameters.OrderBy,
                        pageNumber = eventsResourceParameters.PageNumber - 1,
                        pageSize = eventsResourceParameters.PageSize,
                        name = eventsResourceParameters.Name,
                        searchQuery = eventsResourceParameters.SearchQuery
                });
            case ResourceUriType.NextPage:
                return Url.Link ("GetEvents", new
                {
                    fields = eventsResourceParameters.Fields,
                        orderBy = eventsResourceParameters.OrderBy,
                        pageNumber = eventsResourceParameters.PageNumber + 1,
                        pageSize = eventsResourceParameters.PageSize,
                        name = eventsResourceParameters.Name,
                        searchQuery = eventsResourceParameters.SearchQuery
                });
            case ResourceUriType.Current:
            default:
                return Url.Link ("GetEvents", new
                {
                    fields = eventsResourceParameters.Fields,
                        orderBy = eventsResourceParameters.OrderBy,
                        pageNumber = eventsResourceParameters.PageNumber,
                        pageSize = eventsResourceParameters.PageSize,
                        name = eventsResourceParameters.Name,
                        searchQuery = eventsResourceParameters.SearchQuery
                });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForEvent (Guid eventId, string fields)
        {
            var links = new List<LinkDto> ();

            if (string.IsNullOrWhiteSpace (fields))
            {
                links.Add (
                    new LinkDto (Url.Link ("GetEventById", new { eventId }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add (
                    new LinkDto (Url.Link ("GetEventById", new { eventId, fields }),
                        "self",
                        "GET"));
            }
            links.Add (
                new LinkDto (Url.Link ("DeleteEvent", new { eventId }),
                    "delete_event",
                    "DELETE"));
            links.Add (
                new LinkDto (Url.Link ("GetEvents", new {}),
                    "events",
                    "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForEvents (EventsResourceParameters eventsResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto> ();

            // self 
            links.Add (
                new LinkDto (CreateEventResourceUri (
                    eventsResourceParameters, ResourceUriType.Current), "self", "GET"));
            if (hasNext)
            {
                links.Add (
                    new LinkDto (CreateEventResourceUri (eventsResourceParameters, ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add (
                    new LinkDto (CreateEventResourceUri (eventsResourceParameters, ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }
    }
}
