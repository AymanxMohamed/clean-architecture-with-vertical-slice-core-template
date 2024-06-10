// ReSharper disable ConvertToPrimaryConstructor
using Core.Application.Common.Persistence;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await _dbContext.SaveChangesAsync(cancellationToken);
}