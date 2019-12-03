using System.Collections.Generic;
using EventManager.API.Domain.Entities;
using EventManager.API.Domain.Enums;

namespace EventManager.API.Models
{
    public class CenterForCreationDto
    {
        public string Name { get; set; }
        public int HallCapacity { get; set; }
        public string Location { get; set; }
        public ECenterType Type { get; set; }
        public decimal Price { get; set; }
        public ICollection<Facility> Facilities { get; set; }
    }
}