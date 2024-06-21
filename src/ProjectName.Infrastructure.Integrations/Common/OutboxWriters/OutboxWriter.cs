using ProjectName.Infrastructure.Persistence;

using SharedKernel.IntegrationEvents;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProjectName.Infrastructure.Integrations.Common.OutboxWriters;

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