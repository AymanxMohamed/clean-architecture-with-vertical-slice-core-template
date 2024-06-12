// ReSharper disable ConvertToPrimaryConstructor

using Core.Application.Common.Contexts;
using Core.Application.Common.Persistence;
using Core.Domain.Common.Entities;
using Core.Domain.Common.Interfaces;
using Core.Domain.Common.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly UserContext? _userContext;

    public UnitOfWork(
        ApplicationDbContext dbContext, 
        IDateTimeProvider dateTimeProvider, 
        IUserContextService userContextService)
    {
        _dbContext = dbContext;
        
        _dateTimeProvider = dateTimeProvider;

        _userContext = userContextService.GetUserContext().IsError ? null : userContextService.GetUserContext().Value;
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

                entry.Property(x => x.CreatedById)
                    .CurrentValue = _userContext?.UserId;
            }
            
            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.ModifiedOnUtc)
                    .CurrentValue = _dateTimeProvider.UtcNow;
                
                entry.Property(x => x.ModifiedById)
                    .CurrentValue = _userContext?.UserId;
            }
        }
    }
}