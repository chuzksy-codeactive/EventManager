using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Models;
using EventManager.API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route ("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController (IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException (nameof (userRepository));
            _mapper = mapper ??
                throw new ArgumentNullException (nameof (mapper));
            _config = config;
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

        [HttpPost ("authenticate")]
        public async Task<IActionResult> AuthenticateUser ([FromBody] AuthenticateUserDto user)
        {
            var authUser = await _userRepository.AuthenticateUserAsync (user.Username, user.Password);

            if (authUser == null)
            {
                return BadRequest (new
                {
                    message = "Username or Password is incorrect"
                });
            }

            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_config["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity (new Claim[]
                {
                new Claim (ClaimTypes.Name, authUser.Id.ToString ()),
                }),
                Expires = DateTime.UtcNow.AddDays (7),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            var tokenString = tokenHandler.WriteToken (token);

            authUser.Token = tokenString;

            var userToReturn = _mapper.Map<UserForAuthenticationDto> (authUser);

            return Ok (userToReturn);
        }
    }
}
