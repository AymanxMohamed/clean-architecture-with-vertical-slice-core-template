using ProjectName.Infrastructure.Integrations.Common.BackgroundService;
using ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;
using ProjectName.Infrastructure.Integrations.Common.OutboxWriters;
using ProjectName.Infrastructure.Persistence.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectName.Infrastructure.Integrations;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameInfrastructureIntegrations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddMessagingSupport(configuration);
    }
    
    private static IServiceCollection AddMessagingSupport(this IServiceCollection services, IConfiguration configuration)
    {
        var messageBrokerSettings = configuration
                                        .GetSection(MessageBrokerSettings.Section)
                                        .Get<MessageBrokerSettings>() ?? new MessageBrokerSettings();

        services.AddSingleton(messageBrokerSettings);
        services.AddBackgroundServices();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        return services;
    }
    
    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<PublishIntegrationEventsBackgroundService>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }
}