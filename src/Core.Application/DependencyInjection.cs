using System.Reflection;

using Core.Application.Mediatr.Behaviors;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreApplication(
        this IServiceCollection services, 
        Assembly mediatorServicesAssembly,
        Assembly? fluentValidatorsAssembly = null)
    {
        return services
            .AddMediatrWithCorePipelines(mediatorServicesAssembly)
            .AddFluentValidationValidators(fluentValidatorsAssembly ?? mediatorServicesAssembly);
    }

    private static IServiceCollection AddMediatrWithCorePipelines(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });
        
        services
            .AddScoped(serviceType: typeof(IPipelineBehavior<,>), implementationType: typeof(ValidationBehavior<,>));
        
        return services;
    }

    private static IServiceCollection AddFluentValidationValidators(
        this IServiceCollection services, 
        Assembly validatorsAssembly)
    {
        return services.AddValidatorsFromAssembly(validatorsAssembly);
    }
}