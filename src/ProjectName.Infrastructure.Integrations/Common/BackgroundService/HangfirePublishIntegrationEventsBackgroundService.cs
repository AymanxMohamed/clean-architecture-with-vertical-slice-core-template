using System.Text.Json;

using ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;
using ProjectName.Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SharedKernel.IntegrationEvents;

using Throw;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundService;

public class HangfirePublishIntegrationEventsBackgroundService(
    IIntegrationEventsPublisher integrationEventPublisher,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<HangfirePublishIntegrationEventsBackgroundService> logger)
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public async Task PublishIntegrationEventsFromDbAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var outboxIntegrationEvents = dbContext.OutboxIntegrationEvents.ToList();

        logger.LogInformation(message: "Read a total of {EventsNumber} outbox integration events", outboxIntegrationEvents.Count);

        outboxIntegrationEvents.ForEach(outboxIntegrationEvent =>
        {
            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(outboxIntegrationEvent.EventContent);
            integrationEvent.ThrowIfNull();

            logger.LogInformation(message: "Publishing event of type: {EventType}", integrationEvent.GetType().Name);
            
            integrationEventPublisher.PublishEvent(integrationEvent);
            logger.LogInformation(message: "Integration event published successfully");
        });

        dbContext.RemoveRange(outboxIntegrationEvents);
        await dbContext.SaveChangesAsync();
    }
}