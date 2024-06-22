using Hangfire;

using ProjectName.Infrastructure.Integrations;
using ProjectName.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddProjectNameHostConfigurations();
builder.Services.AddProjectNameAppServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseProjectNameMiddlewarePipeLine();

app.AddHangfireBackgroundJobs();

RecurringJob.AddOrUpdate(
    recurringJobId: "my-test-job-id",
    queue: "my-test-queue",
    methodCall: () => Console.WriteLine("Hello from recurring job"),
    cronExpression: "*/5 * * * *");

app.Run();
