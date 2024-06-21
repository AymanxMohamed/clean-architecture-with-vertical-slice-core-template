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

using RabbitMQ.Client;

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
        
        healthCheckBuilder
            .AddDatabaseHealthChecks(databaseConfigurations)
            .AddRedisHealthChecks(configuration)
            .AddRabbitMqHealthChecks(configuration);
  
        return services;
    }
    
    private static IHealthChecksBuilder AddDatabaseHealthChecks(
        this IHealthChecksBuilder healthCheckBuilder, 
        DatabaseConfigurations databaseConfigurations)
    {
        if (databaseConfigurations.SqlServerEnabled)
        {
            healthCheckBuilder.AddSqlServer(databaseConfigurations.ConnectionString);
        }
        else
        {
            healthCheckBuilder.AddNpgSql(databaseConfigurations.ConnectionString);
        }
        
        return healthCheckBuilder;
    }
    
    private static IHealthChecksBuilder AddRedisHealthChecks(
        this IHealthChecksBuilder healthCheckBuilder, 
        IConfiguration configuration)
    {
        var cachingSettings = configuration.GetSection(key: CachingSettings.SectionName).Get<CachingSettings>();

        if (cachingSettings is { RedisCacheEnabled: true })
        {
            healthCheckBuilder.AddRedis(cachingSettings.RedisServerUrl);
        }

        return healthCheckBuilder;
    }

    private static IHealthChecksBuilder AddRabbitMqHealthChecks(
        this IHealthChecksBuilder healthCheckBuilder, 
        IConfiguration configuration)
    {
        var messageBrokerSettings = configuration.GetSection(
                key: MessageBrokerSettings.Section)
            .Get<MessageBrokerSettings>() ?? new MessageBrokerSettings();
        
        IConnection connection = new ConnectionFactory
        {
            HostName = messageBrokerSettings.HostName,
            Port = messageBrokerSettings.Port,
            UserName = messageBrokerSettings.UserName,
            Password = messageBrokerSettings.Password
        }.CreateConnection();

        healthCheckBuilder.AddRabbitMQ(
            setup: options =>
            {
                options.Connection = connection;
            }, 
            failureStatus: HealthStatus.Degraded);

        return healthCheckBuilder;
    }
}