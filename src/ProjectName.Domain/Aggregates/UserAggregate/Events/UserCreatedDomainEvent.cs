using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Domain.Common.Interfaces;

namespace ProjectName.Domain.Aggregates.UserAggregate.Events;

public record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;