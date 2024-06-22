using Hangfire;

namespace ProjectName.Infrastructure.Integrations.Common.Constants;

public static class HangfireConstants
{
    public const int DefaultRecurringJobsMinutesInterval = 3;
    
    public static readonly List<BackgroundJobServerOptions> SupportedServers = [
        new BackgroundJobServerOptions
        {
            ServerName = "MainServer",
            Queues = Queues.AllQueues
        }

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