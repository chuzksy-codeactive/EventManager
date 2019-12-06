using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

using AutoMapper;

using EventManager.API.Domain.Validators;
using EventManager.API.Models;
using EventManager.API.Options;
using EventManager.API.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace EventManager.API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallerServices (IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings ();
            configuration.Bind (nameof (jwtSettings), jwtSettings);
            services.AddSingleton (jwtSettings);

            services.AddAuthentication (x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddHttpCacheHeaders ((expirationModelOptions) =>
                {
                    expirationModelOptions.MaxAge = 60;
                    expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
                },
                (validationModelOptions) =>
                {
                    validationModelOptions.MustRevalidate = true;
                });
            services.AddResponseCaching ();
            services.AddControllers (setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson (setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver ();
                })
                .AddXmlDataContractSerializerFormatters ();
            services.AddSwaggerGen (sw =>
            {
                sw.SwaggerDoc ("v1", new OpenApiInfo
                {
                    Title = "Event Manager API",
                        Version = "v1"
                });
                sw.AddSecurityDefinition ("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization header using the bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                });

                sw.AddSecurityRequirement (new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string> ()
                    }
                });
            });
            services.Configure<MvcOptions> (config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter> ()?.FirstOrDefault ();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add ("application/vnd.marvin.hateoas+json");
                }
            });
            services.AddTransient<IPropertyMappingService, PropertyMappingService> ();
            services.AddTransient<IPropertyCheckerService, PropertyCheckerService> ();
            services.AddAutoMapper (AppDomain.CurrentDomain.GetAssemblies ());
            services.AddMvc ().AddFluentValidation (config => config.RegisterValidatorsFromAssemblyContaining<Startup> ());
            services.AddTransient<IValidator<UserForCreationDto>, UserForCreationDtoValidator> ();
            services.AddTransient<IValidator<AuthenticateUserDto>, AuthenticateUserDtoValidator> ();
            services.AddTransient<IValidator<CenterForCreationDto>, CenterForCreationDtoValidator> ();
        }
    }
}
