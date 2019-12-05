using System.Collections.Generic;

using EventManager.API.Models;

using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [Route ("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet (Name = "GetRoot")]
        public IActionResult GetRoot ()
        {
            // create links for root
            var links = new List<LinkDto>
            {
            new LinkDto (Url.Link ("GetRoot", new {}),
            "self",
            "GET"),

            new LinkDto (Url.Link ("GetCenters", new {}),
            "centers",
            "GET"),

            new LinkDto (Url.Link ("CreateCenter", new {}),
            "create_centers",
            "POST")
            };

            return Ok (links);
        }
    }
}
