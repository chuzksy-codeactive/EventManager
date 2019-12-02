using System;

namespace EventManager.API.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }
    }
}
