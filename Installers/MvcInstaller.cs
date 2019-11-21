using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace EventManager.API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallerServices (IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers ();
            services.AddSwaggerGen (sw =>
            {
                sw.SwaggerDoc ("v1", new OpenApiInfo
                {
                    Title = "Event Manager API",
                    Version = "v1"
                });
            });
        }
    }
}
