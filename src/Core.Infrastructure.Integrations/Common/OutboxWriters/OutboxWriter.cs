using System.Text.Json;

using Core.Infrastructure.Persistence;

using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Integrations.Common.OutboxWriters;

public class OutboxWriter(ApplicationDbContext dbContext) : IOutboxWriter
{
    public async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
            EventName: integrationEvent.GetType().Name,
            EventContent: JsonSerializer.Serialize(integrationEvent)));

        await dbContext.SaveChangesAsync();
    }
}