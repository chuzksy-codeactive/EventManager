using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Models;
using EventManager.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController (IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException (nameof (userRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers ()
        {
            var userEntities = await _userRepository.GetUsersAsync ();

            return Ok (_mapper.Map<IEnumerable<UserDto>> (userEntities));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser ([FromBody] UserForCreationDto userForCreation)
        {
            if (_userRepository.UserExists (userForCreation.Username, userForCreation.Email))
            {
                return BadRequest ("User already exists in the database");
            }
            var userEntity = _mapper.Map<User> (userForCreation);

            userEntity.Password = BCrypt.Net.BCrypt.HashPassword (userEntity.Password);

            _userRepository.AddUser (userEntity);
            await _userRepository.SaveChangesAsync ();

            var userToReturn = _mapper.Map<UserDto> (userEntity);

            return Ok (userToReturn);
        }
    }
}
