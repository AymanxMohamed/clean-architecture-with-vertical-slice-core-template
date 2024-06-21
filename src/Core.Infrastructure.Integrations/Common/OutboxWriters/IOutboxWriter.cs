using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Integrations.Common.OutboxWriters;

public interface IOutboxWriter
{
    Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent);
}