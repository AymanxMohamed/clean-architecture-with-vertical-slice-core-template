using System.Reflection;

using Asp.Versioning;

using ProjectName.Application;
using ProjectName.Infrastructure;
using ProjectName.Infrastructure.Integrations;
using ProjectName.Infrastructure.Persistence;
using ProjectName.Presentation.Api.Common.OpenApi;

namespace ProjectName.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddProjectNameApiModule(
                    configuration,
                    mediatorServicesAssemblies: [ProjectNameApplicationAssemblyMarker.Assembly, ProjectNameInfrastructureIntegrationsAssemblyMarker.Assembly],
                    controllersAssembly: ProjectNamePresentationAssemblyMarker.Assembly,
                    efProjectNameConfigurationsAssembly: ProjectNameInfrastructurePersistenceAssemblyMarker.Assembly);
    }
    
    public static IServiceCollection AddProjectNameApiModule(
        this IServiceCollection services,
        IConfiguration configuration,
        List<Assembly> mediatorServicesAssemblies,
        Assembly controllersAssembly,
        Assembly efProjectNameConfigurationsAssembly,
        Assembly? mappingAssembly = null,
        List<Assembly>? fluentValidatorsAssemblies = null)
    {
        return services
            .AddProjectNameApplication(mediatorServicesAssemblies, fluentValidatorsAssemblies)
            .AddProjectNameInfrastructure(configuration)
            .AddProjectNameInfrastructurePersistence(configuration, efProjectNameConfigurationsAssembly)
            .AddProjectNameInfrastructureIntegrations(configuration)
            .AddProjectNameApiPresentation(controllersAssembly, mappingAssembly)
            .AddProjectNameThirdParties();
    }

    private static IServiceCollection AddProjectNameThirdParties(this IServiceCollection services)
    {
        return services
            .AddProjectNameApiVersioning()
            .AddSwagger();
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen().ConfigureOptions<ConfigureSwaggerGenOptions>();
    }

    private static IServiceCollection AddProjectNameApiVersioning(this IServiceCollection services)
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