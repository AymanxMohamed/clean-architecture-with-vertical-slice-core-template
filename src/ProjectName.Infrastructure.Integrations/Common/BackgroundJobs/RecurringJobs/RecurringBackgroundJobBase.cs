using Hangfire;

using ProjectName.Application.Common.Services.BackgroundJobs;
using ProjectName.Infrastructure.Integrations.Common.Constants;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.RecurringJobs;

public abstract class RecurringBackgroundJobBase(ICronExpressionGenerator cronExpressionGenerator) : BackgroundJobBase
{
    protected readonly ICronExpressionGenerator _cronExpressionGenerator = cronExpressionGenerator;

    public abstract string GetJobId();

    public virtual string GetCronExpression() => 
        _cronExpressionGenerator.MinutesInterval(HangfireConstants.DefaultRecurringJobsMinutesInterval);

    public override void Run()
    {
        RecurringJob.AddOrUpdate(
            recurringJobId: GetJobId(),
            methodCall: () => ExecuteAsync(),
            cronExpression: GetCronExpression(),
            queue: GetQueueName());
    }
}