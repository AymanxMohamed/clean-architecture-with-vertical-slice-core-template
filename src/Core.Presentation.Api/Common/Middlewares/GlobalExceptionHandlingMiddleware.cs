using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Core.Presentation.Api.Common.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<GlobalExceptionHandlingMiddleware> logger, 
    ProblemDetailsFactory problemDetailsFactory)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred: {Message}, {@Exception}", ex.Message, ex);
            await HandleExceptionAsync(httpContext);
        }
    }

    private Task HandleExceptionAsync(HttpContext context)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;
        var message = $"An unexpected error occurred.";
        const string title = "An error occurred while processing your request.";

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            context, statusCode, detail: message, title: title);

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}