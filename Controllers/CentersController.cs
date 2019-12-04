using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Models;
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

        public CentersController (ICenterRepository centerRepository, IMapper mapper)
        {
            _centerRepository = centerRepository ??
                throw new ArgumentNullException (nameof (centerRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetCenters ()
        {
            var centers = await _centerRepository.GetCentersAsync ();

            var centersToReturn = _mapper.Map<IEnumerable<CenterDto>> (centers);

            return Ok (centersToReturn);
        }

        [HttpPost]
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
        public async Task<IActionResult> GetCenterById (Guid centerId)
        {
            if (string.IsNullOrWhiteSpace (centerId.ToString ()))
            {
                return BadRequest (new
                {
                    message = "User Id should not be null or empty!"
                });
            }

            var center = await _centerRepository.GetCenterByIdAsync (centerId);

            if (center == null)
            {
                return NotFound ();
            }

            var centerToReturn = _mapper.Map<CenterDto> (center);

            return Ok (centerToReturn);
        }

        [HttpDelete ("{centerId}")]
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

            var centerToPatch = _mapper.Map<CenterForUpdateDto>(center);

            patchDocument.ApplyTo(centerToPatch);

            _mapper.Map(centerToPatch, center);

            _centerRepository.UpdateCenter(center);
            await _centerRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
