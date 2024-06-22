using Hangfire;
using Hangfire.PostgreSql;

using ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;
using ProjectName.Infrastructure.Integrations.Common.OutboxWriters;
using ProjectName.Infrastructure.Persistence.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using ProjectName.Application.Common.Services.BackgroundJobs;
using ProjectName.Infrastructure.Common.Services.Caching;
using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.CronExpressions;
using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.RecurringJobs;
using ProjectName.Infrastructure.Integrations.Common.BackgroundService;
using ProjectName.Infrastructure.Integrations.Common.Constants;
using ProjectName.Infrastructure.Persistence.Common.Configurations;

using RabbitMQ.Client;

namespace ProjectName.Infrastructure.Integrations;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameInfrastructureIntegrations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddHangfireSupport(configuration)
            .AddMessagingSupport(configuration)
            .AddHealthChecksSupport(configuration);
    }
    
    private static IServiceCollection AddMessagingSupport(this IServiceCollection services, IConfiguration configuration)
    {
        var messageBrokerSettings = configuration
                                        .GetSection(MessageBrokerSettings.Section)
                                        .Get<MessageBrokerSettings>() ?? new MessageBrokerSettings();

        services.AddSingleton(messageBrokerSettings);
        services.AddHangfireBackgroundJobs();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        return services;
    }
    
    private static void AddBackgroundHostedServices(this IServiceCollection services)
    {
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<PublishIntegrationEventsBackgroundService>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();
    }

    private static void AddHangfireBackgroundJobs(this IServiceCollection services)
    {
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddSingleton<ICronExpressionGenerator, CronExpressionGenerator>();
        services.AddSingleton<PublishIntegrationEventsRecurringJob>();
        services.AddSingleton<ConsumeIntegrationEventsRecurringJob>();
    }

    private static IServiceCollection AddHangfireSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireClient(configuration);

        foreach (BackgroundJobServerOptions server in HangfireConstants.SupportedServers)
        {
            services.AddHangfireServer(options =>
            {
                options.Queues = server.Queues;
                options.ServerName = server.ServerName;
            });
        }

        return services;
    }

    private static void AddHangfireClient(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireDbConfigurations = configuration.GetDbConfigurations().HangfireDbConfigurations;

        services.AddSingleton(hangfireDbConfigurations);

        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer();

            if (hangfireDbConfigurations.SqlServerEnabled)
            {
                config.UseSqlServerStorage(hangfireDbConfigurations.ConnectionString);
            }
            else
            {
                config.UsePostgreSqlStorage(
                    configure: options => options.UseNpgsqlConnection(hangfireDbConfigurations.ConnectionString));
            }
        });
    }
    
    private static IServiceCollection AddHealthChecksSupport(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var healthCheckBuilder = services.AddHealthChecks();
        
        healthCheckBuilder
            .AddDatabaseHealthChecks(configuration.GetDbConfigurations().ApplicationDbConfigurations)
            .AddRedisHealthChecks(configuration)
            .AddRabbitMqHealthChecks(configuration)
            .AddHangfireHealthChecks(configuration);
  
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
    
    private static void AddHangfireHealthChecks(
        this IHealthChecksBuilder healthCheckBuilder,
        IConfiguration configuration)
    {
        healthCheckBuilder.AddDatabaseHealthChecks(configuration.GetDbConfigurations().HangfireDbConfigurations);

        healthCheckBuilder.AddHangfire(
            setup: options =>
            {
                options.MinimumAvailableServers = HangfireConstants.SupportedServers.Count;
            },
            failureStatus: HealthStatus.Degraded);
    }
}