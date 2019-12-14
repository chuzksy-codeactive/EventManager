using System;

namespace EventManager.API.Models
{
    public class EventForCreationDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public string ScheduledDate { get; set; }
    }
}