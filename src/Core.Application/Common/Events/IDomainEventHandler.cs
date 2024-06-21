using Core.Domain.Common.Interfaces;

using MediatR;

namespace Core.Application.Common.Events;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;