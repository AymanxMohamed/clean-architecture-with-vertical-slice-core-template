using Core.Domain.Common.Interfaces;
using Core.Infrastructure.Persistence.Common.Middlewares;
using Core.Infrastructure.Persistence.Common.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using SharedKernel.IntegrationEvents;

namespace Core.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents { get; set; } = null!;
    
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        var domainEvents = GetDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);
        
        EnqueueDomainEvents(domainEvents);

        return result;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(EfCoreConfigurationsAssemblyProvider
            .GetEfCoreConfigurationsAssembly());
    }

    private List<IDomainEvent> GetDomainEvents()
    {
        return ChangeTracker.Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();
    }

    private void EnqueueDomainEvents(List<IDomainEvent> domainEvents)
    {
        var httpContext = httpContextAccessor.HttpContext!;
        
        var domainEventsQueue = GetDomainEventsQueue(httpContext);

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        
        httpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
    }
    
    private static Queue<IDomainEvent> GetDomainEventsQueue(HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) 
            && value is Queue<IDomainEvent> existingDomainEvents)
        {
            return existingDomainEvents;
        }

        return new Queue<IDomainEvent>();
    }
}
