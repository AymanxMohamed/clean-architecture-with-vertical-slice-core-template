using System.Text.Json;

using ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;
using ProjectName.Infrastructure.Persistence;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SharedKernel.IntegrationEvents;

using Throw;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundService;

public class PublishIntegrationEventsBackgroundService(
    IIntegrationEventsPublisher integrationEventPublisher,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<PublishIntegrationEventsBackgroundService> logger)
    : IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    private Task? _doWorkTask;
    private PeriodicTimer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _doWorkTask = DoWorkAsync();

        return Task.CompletedTask;
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_doWorkTask is null)
        {
            return;
        }

        await _cancellationTokenSource.CancelAsync();
        await _doWorkTask;

        _timer?.Dispose();
        _cancellationTokenSource.Dispose();
    }

    private async Task DoWorkAsync()
    {
        logger.LogInformation(message: "Starting integration event publisher background service.");

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
        {
            try
            {
                await PublishIntegrationEventsFromDbAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, message: "Exception occurred while publishing integration events.");
            }
        }
    }

    private async Task PublishIntegrationEventsFromDbAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var outboxIntegrationEvents = dbContext.OutboxIntegrationEvents.ToList();

        logger.LogInformation(message: "Read a total of {EventsNumber} outbox integration events", outboxIntegrationEvents.Count);

        outboxIntegrationEvents.ForEach(outboxIntegrationEvent =>
        {
            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(outboxIntegrationEvent.EventContent);
            integrationEvent.ThrowIfNull();

            logger.LogInformation(message: "Publishing event of type: {EventType}", integrationEvent.GetType().Name);
            
            integrationEventPublisher.PublishEvent(integrationEvent);
            logger.LogInformation(message: "Integration event published successfully");
        });

        dbContext.RemoveRange(outboxIntegrationEvents);
        await dbContext.SaveChangesAsync();
    }
}