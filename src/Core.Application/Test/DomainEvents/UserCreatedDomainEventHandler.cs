using Core.Application.Common.Events;
using Core.Domain.Aggregates.UserAggregate.Events;

namespace Core.Application.Test.DomainEvents;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Domain Event Handler");
        
        await Task.CompletedTask;
    }
}