using System;
using System.Linq;

using EventManager.API.Installers;
using EventManager.API.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventManager.API
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {
            services.InstallServicesInAssembly (Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            var swaggerOptions = new SwaggerOptions ();
            Configuration.GetSection (nameof (SwaggerOptions)).Bind (swaggerOptions);

            app.UseSwagger (option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI (option =>
            {
                option.SwaggerEndpoint (swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseHttpsRedirection ();
            app.UseResponseCaching ();
            app.UseHttpCacheHeaders ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints =>
            {
                endpoints.MapControllers ();
            });
        }
    }
}
