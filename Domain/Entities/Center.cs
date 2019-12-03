using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EventManager.API.Domain.Enums;

namespace EventManager.API.Domain.Entities
{
    public class Center
    {
        public Center ()
        {
            Facilities = new HashSet<Facility> ();
            Events = new HashSet<Event> ();
        }

        public Guid CenterId { get; set; }
        public string Name { get; set; }
        public int HallCapacity { get; set; }
        public string Location { get; set; }
        public ECenterType Type { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public byte[] Picture { get; set; }
        public ICollection<Facility> Facilities { get; private set; }
        public ICollection<Event> Events { get; private set; }
    }
}
