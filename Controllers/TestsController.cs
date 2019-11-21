using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    public class TestsController : ControllerBase
    {
        [HttpGet("api/users")]
        public IActionResult Get ()
        {
            return Ok (new { Name = "Nicky Minaj" });
        }
    }
}
