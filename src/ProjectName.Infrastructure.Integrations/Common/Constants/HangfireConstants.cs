using Hangfire;

namespace ProjectName.Infrastructure.Integrations.Common.Constants;

public static class HangfireConstants
{
    public static readonly List<BackgroundJobServerOptions> SupportedServers = [
        new BackgroundJobServerOptions
        {
            ServerName = "Default",
            Queues = [Queues.DefaultQueue]
        },
        new BackgroundJobServerOptions
        {
            ServerName = "PublishIntegrationEventsServer",
            Queues = [Queues.PublishingIntegrationEventsQueue]
        },
        new BackgroundJobServerOptions
        {
            ServerName = "ConsumeIntegrationEventsServer",
            Queues = [Queues.ConsumingIntegrationEventsQueue]
        },
    ];

    public static class Queues
    {
        public const string DefaultQueue = "default";
        
        public const string PublishingIntegrationEventsQueue = "publishing-integration-events";
    
        public const string ConsumingIntegrationEventsQueue = "consuming-integration-events";

        public static readonly string[] AllQueues = [
            DefaultQueue, 
            PublishingIntegrationEventsQueue, 
            ConsumingIntegrationEventsQueue];
    }
}