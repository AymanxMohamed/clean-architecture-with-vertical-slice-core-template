using System.Reflection;

using ProjectName.Application.Common.Persistence;
using ProjectName.Infrastructure.Common.Services.Caching;
using ProjectName.Infrastructure.Persistence.Common.Extensions;
using ProjectName.Infrastructure.Persistence.Common.Repositories;
using ProjectName.Infrastructure.Persistence.Common.Services;
using ProjectName.Infrastructure.Persistence.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using ProjectName.Infrastructure.Persistence.Common.Configurations;

using RabbitMQ.Client;

using Throw;

namespace ProjectName.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameInfrastructurePersistence(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efProjectNameConfigurationsAssembly)
    {
        return services
            .AddDbContextFromDbConfigurations(configuration, efProjectNameConfigurationsAssembly)
            .AddGenericRepositoryWithSpecification();
    }
    
    private static IServiceCollection AddDbContextFromDbConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efProjectNameConfigurationsAssembly)
    {
        EfProjectNameConfigurationsAssemblyProvider.SetEfProjectNameConfigurationsAssembly(efProjectNameConfigurationsAssembly);
        
        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.ConfigureFromDatabaseConfigurations(configuration.GetDbConfigurations().ApplicationDbConfigurations);
        });

        return services;
    }

    private static IServiceCollection AddGenericRepositoryWithSpecification(this IServiceCollection services)
    {
        services.AddScoped(
            serviceType: typeof(IGenericRepository<,>), 
            implementationType: typeof(GenericRepository<,>));
        
        services.AddScoped(
            serviceType: typeof(ICachedGenericRepository<,>),  
            implementationType: typeof(CachedGenericRepository<,>));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}