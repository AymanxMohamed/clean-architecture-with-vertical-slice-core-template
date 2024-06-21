// ReSharper disable SuspiciousTypeConversion.Global
using ProjectName.Application.Common.Persistence;

using MediatR;

namespace ProjectName.Application.Common.Mediatr.Behaviors;

public class UnitOfWorkBehaviour<TRequest, TResponse>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const string Command = nameof(Command);
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next(); 
        }

        var result = await next();

        dynamic? errorResult = result;

        if (errorResult is not null && !errorResult.IsError)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        return result;
    }

    private static bool IsNotCommand() => !typeof(TRequest).Name.EndsWith(Command);
}