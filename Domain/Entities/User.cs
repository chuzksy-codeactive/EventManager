using System;
using System.Collections.Generic;

namespace EventManager.API.Domain.Entities
{
    public class User
    {
        public User ()
        {
            Events = new HashSet<Event> ();
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
        public ICollection<Event> Events { get; private set; }
    }
}
