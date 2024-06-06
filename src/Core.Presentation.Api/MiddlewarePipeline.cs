using Core.Presentation.Common.Constants.Endpoints;

namespace Core.Presentation.Api;

public static class MiddlewarePipeline
{
    public static WebApplication UseCoreMiddlewarePipeLine(this WebApplication app)
    {
        app.UseExceptionHandler(CoreEndpoints.GlobalErrorHandlingEndPoint);
        app.UseHsts();
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication(); 
        app.UseAuthorization();
        
        app.MapControllers();
        
        return app;
    }
}