using System.Reflection;

using ProjectName.Presentation.Common.Errors;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectName.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameApiPresentation(
        this IServiceCollection services, 
        Assembly controllersAssembly,
        Assembly? mappingAssembly = null)
    {
        services
            .AddControllers(controllersAssembly)
            .AddMappings(mappingAssembly ?? controllersAssembly)
            .AddHttpContextAccessor();
        
        return services;
    }

    private static IServiceCollection AddControllers(
        this IServiceCollection services, 
        Assembly controllersAssembly)
    {
        services
            .AddControllers()
            .AddApplicationPart(ProjectNamePresentationAssemblyMarker.Assembly)
            .AddApplicationPart(controllersAssembly);
        
        services.AddSingleton<ProblemDetailsFactory, AppProblemDetailsFactory>();

        return services;
    }
    
    private static IServiceCollection AddMappings(this IServiceCollection services, Assembly mappingAssembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies: [mappingAssembly, ProjectNamePresentationAssemblyMarker.Assembly]);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}