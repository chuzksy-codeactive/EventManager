using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.API.Installers
{
    public interface IInstaller
    {
         void InstallerServices(IServiceCollection services, IConfiguration configuration);
    }
}