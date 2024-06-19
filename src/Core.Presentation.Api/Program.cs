using Core.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCoreHostConfigurations();
builder.Services.AddCoreAppServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseCoreMiddlewarePipeLine();

app.Run();
