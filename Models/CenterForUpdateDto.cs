using EventManager.API.Domain.Enums;

namespace EventManager.API.Models
{
    public class CenterForUpdateDto
    {
        public string Name { get; set; }
        public int HallCapacity { get; set; }
        public string Location { get; set; }
        public ECenterType Type { get; set; }
        public decimal Price { get; set; }
        public byte?[] Picture { get; set; }
    }
}
