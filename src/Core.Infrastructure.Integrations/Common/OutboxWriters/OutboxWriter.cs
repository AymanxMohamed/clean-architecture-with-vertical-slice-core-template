using Core.Infrastructure.Persistence;

using Newtonsoft.Json;

using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Integrations.Common.OutboxWriters;

public class OutboxWriter(ApplicationDbContext dbContext) : IOutboxWriter
{
    public async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
            EventName: integrationEvent.GetType().Name,
            EventContent: JsonConvert.SerializeObject(integrationEvent)));

        await dbContext.SaveChangesAsync();
    }
}