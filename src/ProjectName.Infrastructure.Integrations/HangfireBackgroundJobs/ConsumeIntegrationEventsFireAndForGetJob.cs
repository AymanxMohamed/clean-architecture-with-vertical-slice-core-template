using System.Text;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.Abstractions;
using ProjectName.Infrastructure.Persistence.Common.Settings;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using SharedKernel.IntegrationEvents;

using Throw;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs;

public class ConsumeIntegrationEventsFireAndForGetJob : FireAndForgetJobBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ConsumeIntegrationEventsFireAndForGetJob> _logger;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IModel _channel;

    public ConsumeIntegrationEventsFireAndForGetJob(
        ILogger<ConsumeIntegrationEventsFireAndForGetJob> logger,
        IServiceScopeFactory serviceScopeFactory,
        MessageBrokerSettings messageBrokerSettings) 
    {
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();
        _serviceScopeFactory = serviceScopeFactory;

        IConnectionFactory connectionFactory = new ConnectionFactory
        {
            HostName = messageBrokerSettings.HostName,
            Port = messageBrokerSettings.Port,
            UserName = messageBrokerSettings.UserName,
            Password = messageBrokerSettings.Password
        };

        IConnection connection = connectionFactory.CreateConnection();

        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true);

        _channel.QueueDeclare(
            queue: messageBrokerSettings.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            messageBrokerSettings.QueueName,
            messageBrokerSettings.ExchangeName,
            routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += PublishIntegrationEvent;

        _channel.BasicConsume(messageBrokerSettings.QueueName, autoAck: false, consumer);
    }

    public override Task ExecuteAsync()
    {
        _logger.LogInformation(message: "Starting integration event consumer background service.");
        return Task.CompletedTask;
    }

    private async void PublishIntegrationEvent(object? sender, BasicDeliverEventArgs eventArgs)
    {
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _logger.LogInformation(message: "Cancellation requested, not consuming integration event.");
            return;
        }

        try
        {
            _logger.LogInformation(message: "Received integration event. Reading message from queue.");

            using var scope = _serviceScopeFactory.CreateScope();

            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(message);
            integrationEvent.ThrowIfNull();

            _logger.LogInformation(
                message: "Received integration event of type: {IntegrationEventType}. Publishing event.",
                integrationEvent.GetType().Name);

            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            await publisher.Publish(integrationEvent);

            _logger.LogInformation(message: "Integration event published in the service successfully. Sending ack to message broker.");

            _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, message: "Exception occurred while consuming integration event");
        }
    }
}