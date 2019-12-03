using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using EventManager.API.Domain.Entities;
using EventManager.API.Models;
using EventManager.API.Services;

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
            if(_centerRepository.CenterExists(centerForCreation.Name))
            {
                return BadRequest("Center already exists in the database");
            }

            var center = _mapper.Map<Center>(centerForCreation);

            _centerRepository.AddCenter(center);
            await _centerRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
