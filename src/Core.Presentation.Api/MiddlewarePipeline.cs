using Core.Infrastructure.Persistence;
using Core.Infrastructure.Persistence.Common.Middlewares;
using Core.Presentation.Common.Constants.Endpoints;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Core.Presentation.Api;

public static class MiddlewarePipeline
{
    public static WebApplication UseCoreMiddlewarePipeLine(this WebApplication app)
    {
        app.UseExceptionHandler(CoreEndpoints.GlobalErrorHandlingEndPoint);
        app.UseMiddleware<EventualConsistencyMiddleware>();
        app.UseHsts();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapHealthChecks(pattern: CoreEndpoints.HealthCheckEndpoint, new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseAuthentication(); 
        app.UseAuthorization();
        
        app.MapControllers();

        app.ApplyDatabasePendingMigrations();
        
        return app;
    }

    private static WebApplication ApplyDatabasePendingMigrations(this WebApplication app)
    {
        var serviceProvider = app.Services;

        using var scope = serviceProvider.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}