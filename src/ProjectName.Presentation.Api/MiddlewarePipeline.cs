using ProjectName.Infrastructure.Persistence;
using ProjectName.Infrastructure.Persistence.Common.Middlewares;
using ProjectName.Presentation.Api.Common.Middlewares;
using ProjectName.Presentation.Common.Constants.Endpoints;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace ProjectName.Presentation.Api;

public static class MiddlewarePipeline
{
    public static WebApplication UseProjectNameMiddlewarePipeLine(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseExceptionHandler(ProjectNameEndpoints.GlobalErrorHandlingEndPoint);

        app.UseMiddleware<EventualConsistencyMiddleware>();
        
        app.UseHsts();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var versions = app.DescribeApiVersions();
            foreach (var version in versions)
            {
                var url = $"/swagger/{version.GroupName}/swagger.json";
                var name = version.GroupName.ToUpperInvariant();
                
                options.SwaggerEndpoint(url, name);
            }
        });
        
        app.UseHttpsRedirection();

        app.MapHealthChecks(pattern: ProjectNameEndpoints.HealthCheckEndpoint, new HealthCheckOptions
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