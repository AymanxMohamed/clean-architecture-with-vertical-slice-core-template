using System.Reflection;

using Core.Application.Common.Persistence;
using Core.Infrastructure.Persistence.Common.Extensions;
using Core.Infrastructure.Persistence.Common.Repositories;
using Core.Infrastructure.Persistence.Common.Services;
using Core.Infrastructure.Persistence.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Core.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreInfrastructurePersistence(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efCoreConfigurationsAssembly)
    {
        return services
            .AddDbContextFromDbConfigurations(configuration, efCoreConfigurationsAssembly)
            .AddGenericRepositoryWithSpecification();
    }
    
    private static IServiceCollection AddDbContextFromDbConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efCoreConfigurationsAssembly)
    {
        EfCoreConfigurationsAssemblyProvider.SetEfCoreConfigurationsAssembly(efCoreConfigurationsAssembly);
        
        var databaseConfigurations = new DatabaseConfigurations();
        
        configuration.GetSection(DatabaseConfigurations.SectionName).Bind(databaseConfigurations);

        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.ConfigureFromDatabaseConfigurations(databaseConfigurations);
        });

        return services.AddHealthChecksSupport(databaseConfigurations);
    }

    private static IServiceCollection AddGenericRepositoryWithSpecification(this IServiceCollection services)
    {
        services.AddTransient(
            serviceType: typeof(IGenericRepository<,>),  
            implementationType: typeof(GenericRepository<,>));

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
    
    private static IServiceCollection AddHealthChecksSupport(
        this IServiceCollection services, 
        DatabaseConfigurations databaseConfigurations)
    {
        var healthCheckBuilder = services.AddHealthChecks();

        if (databaseConfigurations.SqlServerEnabled)
        {
            healthCheckBuilder.AddSqlServer(
                databaseConfigurations.ConnectionString, 
                failureStatus: HealthStatus.Unhealthy);
        }
        else
        {
            healthCheckBuilder.AddNpgSql(
                databaseConfigurations.ConnectionString, 
                failureStatus: HealthStatus.Unhealthy);
        }

        healthCheckBuilder.AddDbContextCheck<ApplicationDbContext>(failureStatus: HealthStatus.Unhealthy);
        
        healthCheckBuilder.AddRedis("Redis Connection string", failureStatus: HealthStatus.Degraded);
        
        return services;
    }
}