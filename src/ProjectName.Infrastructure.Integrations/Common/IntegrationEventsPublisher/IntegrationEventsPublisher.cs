using System.Text;
using System.Text.Json;

using ProjectName.Infrastructure.Persistence.Common.Settings;

using RabbitMQ.Client;

using SharedKernel.IntegrationEvents;

namespace ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;

public class IntegrationEventsPublisher : IIntegrationEventsPublisher
{
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly IModel _channel;

    public IntegrationEventsPublisher(MessageBrokerSettings messageBrokerSettings)
    {
        _messageBrokerSettings = messageBrokerSettings;
        IConnectionFactory connectionFactory = new ConnectionFactory
        {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password
        };

        IConnection connection = connectionFactory.CreateConnection();

        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(
            _messageBrokerSettings.ExchangeName,
            ExchangeType.Fanout,
            durable: true);
    }

    public void PublishEvent(IIntegrationEvent integrationEvent)
    {
        string serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent);

        byte[] body = Encoding.UTF8.GetBytes(serializedIntegrationEvent);

        _channel.BasicPublish(
            exchange: _messageBrokerSettings.ExchangeName,
            routingKey: string.Empty,
            body: body);
    }
}