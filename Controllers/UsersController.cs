using System;
using System.Linq;
using System.Threading.Tasks;

using EventManager.API.Helpers;
using EventManager.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController (IUserRepository userRepository)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException (nameof (userRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAsync ()
        {
            var userEntities = await _userRepository.GetUsersAsync ();

            return Ok (userEntities);
        }
    }
}
