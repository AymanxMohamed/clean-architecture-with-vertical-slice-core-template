using ProjectName.Domain.Common.Services;

using MediatR;

using Microsoft.Extensions.Logging;

namespace ProjectName.Application.Common.Mediatr.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger, IDateTimeProvider dateTimeProvider) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var dateTimeUtc = dateTimeProvider.UtcNow;

        logger.LogInformation(
            message: "Starting request {@RequestName}, {@DateTimeUtc}", requestName, dateTimeUtc);
        
        var result = await next();
        
        if (result.IsError)
        {
            var error = result.Errors?.FirstOrDefault();
            
            var logLevel = error?.Type switch
            {
                ErrorType.Unexpected => LogLevel.Critical,
                ErrorType.Failure => LogLevel.Error,
                ErrorType.Forbidden => LogLevel.Warning,
                _ => LogLevel.Information
            };

            logger.Log(
                logLevel,
                "Request Failure {@RequestName}, {@Errors} {@DateTimeUtc}",
                requestName,
                result.Errors,
                dateTimeUtc);
        }
        
        logger.LogInformation(
            message: "Completing request {@RequestName} {@DateTimeUtc}", requestName, dateTimeUtc);

        return result;
    }
}