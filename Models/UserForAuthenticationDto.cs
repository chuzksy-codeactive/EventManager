using System;

namespace EventManager.API.Models
{
    public class UserForAuthenticationDto
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}