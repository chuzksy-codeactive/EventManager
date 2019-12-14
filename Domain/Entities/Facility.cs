using System;

namespace EventManager.API.Domain.Entities
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid CenterId { get; set; }
        public Center Center { get; set; }
    }
}
