using Microsoft.EntityFrameworkCore;

namespace EventManager.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options) : base (options)
        {}
    }
}
