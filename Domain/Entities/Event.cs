using System;
using System.Collections.Generic;

namespace EventManager.API.Domain.Entities
{
    public class Event
    {
        public Event ()
        {
            Users = new HashSet<User> ();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
        public DateTimeOffset ScheduledDate { get; set; }
        public ICollection<User> Users { get; private set; }
    }
}
