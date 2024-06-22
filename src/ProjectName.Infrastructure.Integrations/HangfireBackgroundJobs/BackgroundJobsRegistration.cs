using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs;

public static class BackgroundJobsRegistration
{
    public static IApplicationBuilder AddHangfireBackgroundJobs(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices;
        serviceProvider.GetRequiredService<PublishIntegrationEventsRecurringJob>().Run();
        serviceProvider.GetRequiredService<ConsumeIntegrationEventsFireAndForGetJob>().Run();
        return app;
    }
}