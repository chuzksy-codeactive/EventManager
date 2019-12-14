using System;

namespace EventManager.API.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public string ScheduledDate { get; set; }

        public Guid CenterId { get; set; }
        public Center Center { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
