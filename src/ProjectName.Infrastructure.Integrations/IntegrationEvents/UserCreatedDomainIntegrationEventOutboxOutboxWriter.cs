using ProjectName.Domain.Aggregates.UserAggregate.Events;
using ProjectName.Infrastructure.Integrations.Common.OutboxWriters;

using SharedKernel.IntegrationEvents.UserManagement;

namespace ProjectName.Infrastructure.Integrations.IntegrationEvents;

public class UserCreatedDomainIntegrationEventOutboxOutboxWriter(IOutboxWriter outboxWriter) 
    : IntegrationEventOutboxWriter<UserCreatedDomainEvent, UserCreatedIntegrationEvent>(outboxWriter)
{
    protected override UserCreatedIntegrationEvent GenerateIntegrationEvent(UserCreatedDomainEvent domainEvent)
    {
        return new UserCreatedIntegrationEvent(domainEvent.UserId.Value);
    }
}

