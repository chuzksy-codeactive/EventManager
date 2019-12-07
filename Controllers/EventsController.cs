using System;
using System.Collections.Generic;
using System.Text.Json;

using AutoMapper;
using EventManager.API.Helpers;
using EventManager.API.Models;
using EventManager.API.ResourceParameters;
using EventManager.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventsController (
            IEventRepository eventRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository ??
                throw new ArgumentNullException (nameof (eventRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        [HttpGet (Name = "GetEvents")]
        public IActionResult GetEvents ([FromQuery] EventsResourceParameters eventsResourceParameters)
        {
            var events = _eventRepository.GetEvents (eventsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = events.TotalCount,
                pageSize = events.PageSize,
                currentPage = events.CurrentPage,
                totalPages = events.TotalPages
            };

            Response.Headers.Add ("X-Pagination", JsonSerializer.Serialize (paginationMetadata));

            var shapeEvents = _mapper.Map<IEnumerable<EventDto>>(events).ShapeData(eventsResourceParameters.Fields);

            return Ok(shapeEvents);
        }
    }
}
