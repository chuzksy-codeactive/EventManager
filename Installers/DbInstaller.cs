using EventManager.API.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallerServices (IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext> (options =>
                options.UseSqlServer (
                    configuration.GetConnectionString ("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser> ()
                .AddEntityFrameworkStores<DataContext> ();
        }
    }
}
