using System.Reflection;

using Core.Application.Common.Mediatr.Behaviors;
using Core.Application.Common.Persistence;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services, 
        List<Assembly> mediatorAssemblies,
        List<Assembly>? fluentValidatorsAssemblies = null)
    {
        return services
            .AddMediatrWithCorePipelines(mediatorAssemblies)
            .AddFluentValidationValidators(fluentValidatorsAssemblies ?? mediatorAssemblies);
    }

    private static IServiceCollection AddMediatrWithCorePipelines(this IServiceCollection services, List<Assembly> assemblies)
    {
        assemblies.Add(CoreApplicationAssemblyMarker.Assembly);
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
        validatorsAssemblies.Add(CoreApplicationAssemblyMarker.Assembly);
        
        return services.AddValidatorsFromAssemblies(validatorsAssemblies);
    }
}