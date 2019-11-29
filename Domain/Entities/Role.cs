using System;

namespace EventManager.API.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string UserRole { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
