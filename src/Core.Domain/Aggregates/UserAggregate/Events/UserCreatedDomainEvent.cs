using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Interfaces;

namespace Core.Domain.Aggregates.UserAggregate.Events;

public record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;