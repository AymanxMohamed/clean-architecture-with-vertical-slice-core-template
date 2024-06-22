using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ProjectName.Application.Common.Services.BackgroundJobs;
using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.Abstractions;
using ProjectName.Infrastructure.Integrations.Common.Constants;
using ProjectName.Infrastructure.Integrations.Common.IntegrationEventsPublisher;
using ProjectName.Infrastructure.Persistence;

using SharedKernel.IntegrationEvents;

using Throw;

namespace ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs;

public class PublishIntegrationEventsRecurringJob(
    IIntegrationEventsPublisher integrationEventPublisher,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<PublishIntegrationEventsRecurringJob> logger,
    ICronExpressionGenerator cronExpressionGenerator) : RecurringFireAndForgetJobBase(cronExpressionGenerator)
{
    private const string JobId = "publish-integration-events-job";
    
    public override string GetQueueName() => HangfireConstants.Queues.PublishingIntegrationEventsQueue;

    public override string GetJobId() => JobId;

    public override string GetCronExpression() => _cronExpressionGenerator.SecondsInterval(5);

    public override async Task ExecuteAsync()
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