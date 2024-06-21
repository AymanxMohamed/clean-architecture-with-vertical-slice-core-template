using System.Reflection;

using Core.Application.Common.Persistence;
using Core.Infrastructure.Common.Services.Caching;
using Core.Infrastructure.Persistence.Common.Extensions;
using Core.Infrastructure.Persistence.Common.Repositories;
using Core.Infrastructure.Persistence.Common.Services;
using Core.Infrastructure.Persistence.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        return services.AddHealthChecksSupport(configuration, databaseConfigurations);
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
    
    private static IServiceCollection AddHealthChecksSupport(
        this IServiceCollection services, 
        IConfiguration configuration,
        DatabaseConfigurations databaseConfigurations)
    {
        var healthCheckBuilder = services.AddHealthChecks();

        if (databaseConfigurations.SqlServerEnabled)
        {
            healthCheckBuilder.AddSqlServer(databaseConfigurations.ConnectionString);
        }
        else
        {
            healthCheckBuilder.AddNpgSql(databaseConfigurations.ConnectionString);
        }

        var cachingSettings = configuration.GetSection(key: CachingSettings.SectionName).Get<CachingSettings>();

        if (cachingSettings is { RedisCacheEnabled: true })
        {
            healthCheckBuilder.AddRedis(cachingSettings.RedisServerUrl);
        }
        
        return services;
    }
}