using System.Reflection;

using Core.Presentation.Common.Errors;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreApiPresentation(
        this IServiceCollection services, 
        Assembly controllersAssembly,
        Assembly? mappingAssembly = null)
    {
        services
            .AddControllers(controllersAssembly)
            .AddMappings(mappingAssembly ?? controllersAssembly);
        
        return services;
    }

    private static IServiceCollection AddControllers(
        this IServiceCollection services, 
        Assembly controllersAssembly)
    {
        services
            .AddControllers()
            .AddApplicationPart(CorePresentationAssemblyMarker.Assembly)
            .AddApplicationPart(controllersAssembly);
        
        services.AddSingleton<ProblemDetailsFactory, AppProblemDetailsFactory>();

        return services;
    }
    
    private static IServiceCollection AddMappings(this IServiceCollection services, Assembly mappingAssembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies: [mappingAssembly, CorePresentationAssemblyMarker.Assembly]);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}