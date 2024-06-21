// ReSharper disable ConvertToPrimaryConstructor

using ProjectName.Application.Common.Contexts;
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Common.Entities;
using ProjectName.Domain.Common.Interfaces;
using ProjectName.Domain.Common.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProjectName.Infrastructure.Persistence.Common.Repositories;

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