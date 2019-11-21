using EventManager.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace EventManager.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options) : base (options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<Facility> Facilities { get; set; }
    }
}
