using MediatR;

using Microsoft.Extensions.Logging;

namespace ProjectName.Application.Common.Mediatr.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured: {Message}, {@Exception}", ex.Message, ex);
            return (dynamic)Error.Unexpected(description: ex.Message);
        }
    }
}