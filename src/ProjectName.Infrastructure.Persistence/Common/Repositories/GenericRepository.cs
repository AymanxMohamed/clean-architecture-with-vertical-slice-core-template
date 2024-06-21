// ReSharper disable ConvertToPrimaryConstructor
// ReSharper disable MemberCanBePrivate.Global
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Common.Models;
using ProjectName.Domain.Common.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace ProjectName.Infrastructure.Persistence.Common.Repositories;

public class GenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    protected readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetEntryReadOnly()
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking();
        return query;
    }

    public IQueryable<TEntity> GetEntry()
    {
        var query = _dbContext.Set<TEntity>();
        return query;
    }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> specification, 
        bool countAllExcludingPagination = false,
        CancellationToken cancellationToken = default)
    {
        if (countAllExcludingPagination)
        {
            specification.IsPagingEnabled = false;
        }
        
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        var items = await ApplySpecification(specification).ToListAsync(cancellationToken);
        return items;
    }

    public async Task<List<TEntity>> GetReadyOnlyAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        var items = await ApplySpecificationReadOnly(specification).ToListAsync(cancellationToken);
        return items;
    }

    public async Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        var entities = await ApplySpecificationReadOnly(specification).ToListAsync(cancellationToken);
        
        return new PaginationResult<TEntity, TEntityId>(
            items: entities, 
            totalCount: await CountAsync(specification, countAllExcludingPagination: true, cancellationToken), 
            filter: specification.ResourceParameter); 
    }

    public async Task<TEntity?> GetFirstOrDefault(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default) 
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetFirstOrDefaultReadyOnly(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecificationReadOnly(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> CheckExistAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    public async Task<bool> CheckExistByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(Func<TEntity, bool> predicate)
    {
        _dbContext.Set<TEntity>().RemoveRange(_dbContext.Set<TEntity>().Where(predicate));
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
    }

    private IQueryable<TEntity> ApplySpecificationReadOnly(ISpecification<TEntity, TEntityId> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec).AsNoTracking();
    }
}