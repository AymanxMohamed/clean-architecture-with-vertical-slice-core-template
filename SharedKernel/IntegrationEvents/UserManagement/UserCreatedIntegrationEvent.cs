namespace SharedKernel.IntegrationEvents.UserManagement;

public record UserCreatedIntegrationEvent(Guid UserId) : IIntegrationEvent;