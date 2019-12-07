using System;

namespace EventManager.API.Models
{
    public class EventDto
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }

    }
}