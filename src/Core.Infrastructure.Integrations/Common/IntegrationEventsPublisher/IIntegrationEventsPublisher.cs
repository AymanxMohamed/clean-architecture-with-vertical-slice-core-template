using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Integrations.Common.IntegrationEventsPublisher;

public interface IIntegrationEventsPublisher
{
    public void PublishEvent(IIntegrationEvent integrationEvent);
}