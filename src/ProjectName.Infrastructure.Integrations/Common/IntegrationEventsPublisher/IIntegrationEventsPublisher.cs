using SharedKernel.IntegrationEvents;

namespace ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;

public interface IIntegrationEventsPublisher
{
    public void PublishEvent(IIntegrationEvent integrationEvent);
}