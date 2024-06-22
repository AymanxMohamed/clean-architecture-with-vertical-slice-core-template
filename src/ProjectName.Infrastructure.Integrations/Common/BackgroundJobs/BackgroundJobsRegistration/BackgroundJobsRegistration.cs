using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.RecurringJobs;

namespace ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.BackgroundJobsRegistration;

public static class BackgroundJobsRegistration
{
    public static IApplicationBuilder AddHangfireBackgroundJobs(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices;
        serviceProvider.GetRequiredService<PublishIntegrationEventsRecurringJob>().Run();
        serviceProvider.GetRequiredService<ConsumeIntegrationEventsRecurringJob>().Run();
        return app;
    }
}