using System.Reflection;

using Asp.Versioning;

using Core.Application;
using Core.Infrastructure;
using Core.Infrastructure.Persistence;
using Core.Presentation.Api.Common.OpenApi;

using Microsoft.OpenApi.Models;

namespace Core.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddCoreApiModule(
                    configuration,
                    mediatorServicesAssembly: CoreApplicationAssemblyMarker.Assembly,
                    controllersAssembly: CorePresentationAssemblyMarker.Assembly,
                    efCoreConfigurationsAssembly: CoreInfrastructurePersistenceAssemblyMarker.Assembly);
    }
    
    public static IServiceCollection AddCoreApiModule(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly mediatorServicesAssembly,
        Assembly controllersAssembly,
        Assembly efCoreConfigurationsAssembly,
        Assembly? mappingAssembly = null,
        Assembly? fluentValidatorsAssembly = null)
    {
        return services
            .AddCoreApplication(mediatorServicesAssembly, fluentValidatorsAssembly)
            .AddCoreInfrastructure(configuration)
            .AddCoreInfrastructurePersistence(configuration, efCoreConfigurationsAssembly)
            .AddCoreApiPresentation(controllersAssembly, mappingAssembly)
            .AddCoreThirdParties();
    }

    private static IServiceCollection AddCoreThirdParties(this IServiceCollection services)
    {
        return services
            .AddCoreApiVersioning()
            .AddSwagger();
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core", Version = "v1" });
        
            c.AddSecurityDefinition($"Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
        
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }).ConfigureOptions<ConfigureSwaggerGenOptions>();
    }

    private static IServiceCollection AddCoreApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(majorVersion: 1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
        return services;
    }
}