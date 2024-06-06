using Core.Presentation.Api;

using SampleProject.Application;
using SampleProject.Infrastructure;
using SampleProject.Infrastructure.Persistence;

namespace SampleProject.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddExternalModules(configuration)
            .AddAppLayers();
    }

    private static IServiceCollection AddExternalModules(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCoreApiModule(
                configuration,
                mediatorServicesAssembly: ApplicationAssemblyMarker.Assembly,
                controllersAssembly: PresentationAssemblyMarker.Assembly,
                efCoreConfigurationsAssembly: InfrastructurePersistenceAssemblyMarker.Assembly);
    }

    private static IServiceCollection AddAppLayers(this IServiceCollection services)
    {
        return services
            .AddApplication()
            .AddInfrastructure()
            .AddPresentation();
    }
}