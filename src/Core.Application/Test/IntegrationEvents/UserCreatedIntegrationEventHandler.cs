using Microsoft.Extensions.Logging;

using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.UserManagement;

namespace Core.Application.Test.IntegrationEvents;

public class UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            message: "new user has been created and has been processed by the event handler {UserId}",
            notification.UserId);
        
        await Task.CompletedTask;
    }
}