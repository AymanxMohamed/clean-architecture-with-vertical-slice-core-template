using Core.Presentation.Api;

using SampleProject.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

app.UseCoreMiddlewarePipeLine();

app.Run();
