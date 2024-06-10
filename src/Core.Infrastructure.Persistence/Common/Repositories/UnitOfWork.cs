// ReSharper disable ConvertToPrimaryConstructor
using Core.Application.Common.Persistence;
using Core.Domain.Common.Interfaces;
using Core.Domain.Common.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UnitOfWork(ApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x => x.CreatedOnUtc)
                    .CurrentValue = _dateTimeProvider.UtcNow;
            }
            
            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.ModifiedOnUtc)
                    .CurrentValue = _dateTimeProvider.UtcNow;
            }
        }
    }
}