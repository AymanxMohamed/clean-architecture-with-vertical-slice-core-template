using Hangfire;

using ProjectName.Application.Common.Services;
using ProjectName.Application.Common.Services.BackgroundJobs;
using ProjectName.Infrastructure.Integrations.Common.Constants;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.RecurringJobs;

public abstract class BackgroundJobBase : IBackgroundJob
{
    public virtual string GetQueueName() => HangfireConstants.Queues.DefaultQueue;

    public abstract Task ExecuteAsync();
    
    public virtual void Run()
    {
        BackgroundJob.Enqueue(
            methodCall: () => ExecuteAsync(),
            queue: GetQueueName());
    }
}