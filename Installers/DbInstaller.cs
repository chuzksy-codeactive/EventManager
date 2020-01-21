using System;

using EventManager.API.Data;
using EventManager.API.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallerServices (IServiceCollection services, IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<DataContext> (options =>
                    options.UseSqlServer (
                        configuration.GetConnectionString ("AzureConnection")));
            }
            else
            {
                services.AddDbContext<DataContext> (options =>
                    options.UseSqlServer (
                        configuration.GetConnectionString ("DefaultConnection")));
            }
            services.BuildServiceProvider ().GetService<DataContext> ().Database.Migrate ();
            services.AddScoped<IUserRepository, UserRepository> ();
            services.AddScoped<ICenterRepository, CenterRepository> ();
            services.AddScoped<IEventRepository, EventRepository> ();
        }
    }
}
