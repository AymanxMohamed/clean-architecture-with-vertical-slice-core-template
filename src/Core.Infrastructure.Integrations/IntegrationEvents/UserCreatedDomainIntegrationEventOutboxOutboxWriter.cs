using Core.Domain.Aggregates.UserAggregate.Events;
using Core.Infrastructure.Integrations.Common.OutboxWriters;

using SharedKernel.IntegrationEvents.UserManagement;

namespace Core.Infrastructure.Integrations.IntegrationEvents;

public class UserCreatedDomainIntegrationEventOutboxOutboxWriter(IOutboxWriter outboxWriter) 
    : IntegrationEventOutboxWriter<UserCreatedDomainEvent, UserCreatedIntegrationEvent>(outboxWriter)
{
    protected override UserCreatedIntegrationEvent GenerateIntegrationEvent(UserCreatedDomainEvent domainEvent)
    {
        return new UserCreatedIntegrationEvent(domainEvent.UserId.Value);
    }
}

