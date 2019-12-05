using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.Models;
using EventManager.API.ResourceParameters;
using EventManager.API.Services;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/centers")]
    public class CentersController : ControllerBase
    {
        private readonly ICenterRepository _centerRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public CentersController (
            ICenterRepository centerRepository,
            IMapper mapper,
            IPropertyCheckerService propertyCheckerService,
            IPropertyMappingService propertyMappingService)
        {
            _centerRepository = centerRepository ??
                throw new ArgumentNullException (nameof (centerRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException (nameof (propertyMappingService));
            _propertyCheckerService = propertyCheckerService ??
                throw new ArgumentNullException (nameof (propertyCheckerService));
        }

        [HttpGet (Name = "GetCenters")]
        public IActionResult GetCenters ([FromQuery] CentersResourceParameters centersResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CenterDto, Center> (centersResourceParameters.OrderBy))
            {
                return BadRequest ();
            }

            if (!_propertyCheckerService.TypeHasProperties<CenterDto> (centersResourceParameters.Fields))
            {
                return BadRequest ();
            }

            var centers = _centerRepository.GetCenters (centersResourceParameters);

            var previousPageLink = centers.HasPrevious ?
                CreateCenterResourceUri (centersResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = centers.HasNext ?
                CreateCenterResourceUri (centersResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = centers.TotalCount,
                pageSize = centers.PageSize,
                currentPage = centers.CurrentPage,
                totalPages = centers.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add ("X-Pagination", JsonSerializer.Serialize (paginationMetadata));

            return Ok (_mapper.Map<IEnumerable<CenterDto>> (centers).ShapeData (centersResourceParameters.Fields));
        }

        [HttpPost (Name = "CreateCenter")]
        public async Task<IActionResult> CreateCenter ([FromBody] CenterForCreationDto centerForCreation)
        {
            if (_centerRepository.CenterExists (centerForCreation.Name))
            {
                return BadRequest ("Center already exists in the database");
            }

            var center = _mapper.Map<Center> (centerForCreation);

            _centerRepository.AddCenter (center);
            await _centerRepository.SaveChangesAsync ();

            var centerToReturn = _mapper.Map<CenterDto> (center);

            return CreatedAtRoute ("GetCenterById", new { centerId = center.CenterId }, centerToReturn);
        }

        [HttpGet ("{centerId}", Name = "GetCenterById")]
        public async Task<IActionResult> GetCenterById (Guid centerId, string fields)
        {
            if (string.IsNullOrWhiteSpace (centerId.ToString ()))
            {
                return BadRequest (new
                {
                    message = "User Id should not be null or empty!"
                });
            }

            if (!_propertyCheckerService.TypeHasProperties<CenterDto> (fields))
            {
                return BadRequest ();
            }

            var center = await _centerRepository.GetCenterByIdAsync (centerId);

            if (center == null)
            {
                return NotFound ();
            }

            var links = CreateLinksForCenter (centerId, fields);

            var linkedResourceToReturn = _mapper.Map<CenterDto> (center).ShapeData (fields) as IDictionary<string, object>;

            linkedResourceToReturn.Add ("links", links);

            return Ok (linkedResourceToReturn);
        }

        [HttpDelete ("{centerId}", Name = "DeleteCenter")]
        public async Task<IActionResult> DeleteCenter (Guid centerId)
        {
            var center = await _centerRepository.GetCenterByIdAsync (centerId);

            if (center == null)
            {
                return NotFound ();
            }

            _centerRepository.DeleteCenter (center);
            await _centerRepository.SaveChangesAsync ();

            return NoContent ();
        }

        [HttpPut ("{centerId}")]
        public async Task<IActionResult> UpdateCenter (Guid centerId, CenterForUpdateDto centerForUpdate)
        {
            var center = await _centerRepository.GetCenterByIdAsync (centerId);

            if (center == null)
            {
                return NotFound ();
            }

            // map the entity to the courseForUpdateDto
            // apply the updated fields value to that Dto
            // map the courseForUpdateDto back to an entity
            _mapper.Map (centerForUpdate, center);

            _centerRepository.UpdateCenter (center);
            await _centerRepository.SaveChangesAsync ();

            return NoContent ();
        }

        [HttpPatch ("{centerId}")]
        public async Task<IActionResult> PartiallyUpdateCenter (Guid centerId, JsonPatchDocument<CenterForUpdateDto> patchDocument)
        {
            var center = await _centerRepository.GetCenterByIdAsync (centerId);

            if (center == null)
            {
                return NotFound ();
            }

            var centerToPatch = _mapper.Map<CenterForUpdateDto> (center);

            patchDocument.ApplyTo (centerToPatch, ModelState);

            if (!TryValidateModel (centerToPatch))
            {
                return ValidationProblem (ModelState);
            }

            _mapper.Map (centerToPatch, center);

            _centerRepository.UpdateCenter (center);
            await _centerRepository.SaveChangesAsync ();

            return NoContent ();
        }

        private string CreateCenterResourceUri (CentersResourceParameters centersResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
            case ResourceUriType.PreviousPage:
                return Url.Link ("GetCenters", new
                {
                    fields = centersResourceParameters.Fields,
                        orderBy = centersResourceParameters.OrderBy,
                        pageNumber = centersResourceParameters.PageNumber - 1,
                        pageSize = centersResourceParameters.PageSize,
                        name = centersResourceParameters.Name,
                        searchQuery = centersResourceParameters.SearchQuery
                });
            case ResourceUriType.NextPage:
                return Url.Link ("GetCenters", new
                {
                    fields = centersResourceParameters.Fields,
                        orderBy = centersResourceParameters.OrderBy,
                        pageNumber = centersResourceParameters.PageNumber + 1,
                        pageSize = centersResourceParameters.PageSize,
                        name = centersResourceParameters.Name,
                        searchQuery = centersResourceParameters.SearchQuery
                });
            default:
                return Url.Link ("GetCenters", new
                {
                    fields = centersResourceParameters.Fields,
                        orderBy = centersResourceParameters.OrderBy,
                        pageNumber = centersResourceParameters.PageNumber,
                        pageSize = centersResourceParameters.PageSize,
                        name = centersResourceParameters.Name,
                        searchQuery = centersResourceParameters.SearchQuery
                });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForCenter (Guid centerId, string fields)
        {
            var links = new List<LinkDto> ();

            if (string.IsNullOrWhiteSpace (fields))
            {
                links.Add (
                    new LinkDto (Url.Link ("GetCenterById", new { centerId }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add (
                    new LinkDto (Url.Link ("GetCenterById", new { centerId, fields }),
                        "self",
                        "GET"));
            }
            links.Add (
                new LinkDto (Url.Link ("DeleteCenter", new { centerId }),
                    "delete_center",
                    "DELETE"));
            links.Add (
                new LinkDto (Url.Link ("CreateCenter", null),
                    "create_center",
                    "POST"));
            links.Add (
                new LinkDto (Url.Link ("GetCenters", null),
                    "centers",
                    "GET"));

            return links;
        }
    }
}
