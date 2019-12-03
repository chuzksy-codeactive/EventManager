using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

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
    }
}
