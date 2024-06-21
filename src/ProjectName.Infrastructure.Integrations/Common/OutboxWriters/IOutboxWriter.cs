using SharedKernel.IntegrationEvents;

namespace ProjectName.Infrastructure.Integrations.Common.OutboxWriters;

public interface IOutboxWriter
{
    Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent);
}