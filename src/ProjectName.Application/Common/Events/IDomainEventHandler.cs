using ProjectName.Domain.Common.Interfaces;

using MediatR;

namespace ProjectName.Application.Common.Events;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;