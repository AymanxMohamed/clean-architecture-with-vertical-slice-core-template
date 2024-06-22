using ProjectName.Infrastructure.Integrations.Common.BackgroundJobs.BackgroundJobsRegistration;
using ProjectName.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddProjectNameHostConfigurations();
builder.Services.AddProjectNameAppServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseProjectNameMiddlewarePipeLine();

app.AddHangfireBackgroundJobs();

app.Run();
