using ProjectName.Application.Common.Events;
using ProjectName.Domain.Aggregates.UserAggregate.Events;

namespace ProjectName.Application.Test.DomainEvents;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Domain Event Handler");
        
        await Task.CompletedTask;
    }
}