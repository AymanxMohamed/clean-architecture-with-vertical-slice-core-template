using System.Reflection;

using ProjectName.Application.Common.Mediatr.Behaviors;
using ProjectName.Application.Common.Persistence;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace ProjectName.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameApplication(
        this IServiceCollection services, 
        List<Assembly> mediatorAssemblies,
        List<Assembly>? fluentValidatorsAssemblies = null)
    {
        return services
            .AddMediatrWithProjectNamePipelines(mediatorAssemblies)
            .AddFluentValidationValidators(fluentValidatorsAssemblies ?? mediatorAssemblies);
    }

    private static IServiceCollection AddMediatrWithProjectNamePipelines(this IServiceCollection services, List<Assembly> assemblies)
    {
        assemblies.Add(ProjectNameApplicationAssemblyMarker.Assembly);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assemblies.ToArray());
            configuration.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            configuration.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            configuration.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
        });
        
        services
            .AddScoped(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(ValidationBehavior<,>));
        
        return services;
    }

    private static IServiceCollection AddFluentValidationValidators(
        this IServiceCollection services, 
        ICollection<Assembly> validatorsAssemblies)
    {
        validatorsAssemblies.Add(ProjectNameApplicationAssemblyMarker.Assembly);
        
        return services.AddValidatorsFromAssemblies(validatorsAssemblies);
    }
}