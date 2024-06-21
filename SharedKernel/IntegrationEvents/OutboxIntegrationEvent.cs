namespace SharedKernel.IntegrationEvents;

public record OutboxIntegrationEvent(string EventName, string EventContent);