using ProjectName.Domain.Common.EventualConsistency;
using ProjectName.Domain.Common.Interfaces;
using ProjectName.Presentation.Common.Constants.Endpoints;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace ProjectName.Infrastructure.Persistence.Common.Middlewares;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, ApplicationDbContext dbContext)
    {
        if (context.Request.Path.Equals(other: ProjectNameEndpoints.HealthCheckEndpoint, StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var transaction = await dbContext.Database.BeginTransactionAsync();
        
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value) && value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (EventualConsistencyException)
            {
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await next(context);
    }
}
