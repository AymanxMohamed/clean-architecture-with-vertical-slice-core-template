using Core.Domain.Common.Interfaces;

using MediatR;

using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Integrations.Common.OutboxWriters;

public abstract class IntegrationEventOutboxWriter<TDomainEvent, TIntegrationEvent>(IOutboxWriter outboxWriter) 
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
    where TIntegrationEvent : IIntegrationEvent
{
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = GenerateIntegrationEvent(notification);

        await outboxWriter.AddOutboxIntegrationEventAsync(integrationEvent);
    }
    
    protected abstract TIntegrationEvent GenerateIntegrationEvent(TDomainEvent domainEvent);
}