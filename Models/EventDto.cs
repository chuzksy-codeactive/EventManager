using System;

namespace EventManager.API.Models
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public string ScheduledDate { get; set; }

    }
}