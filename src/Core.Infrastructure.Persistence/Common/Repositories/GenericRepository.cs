using Core.Application.Common.Persistence;
using Core.Domain.Common.Persistence.Models;
using Core.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class GenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    protected readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

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

    public async Task<int> CountAllAsync()
    {
        return await _dbContext.Set<TEntity>().CountAsync();
    }

    public async Task<int> CountAsync(ISpecification<TEntity> specification, bool countAllExcludingPagination = false)
    {
        if (countAllExcludingPagination)
        {
            specification.IsPagingEnabled = false;
        }
        
        return await ApplySpecification(specification).CountAsync();
    }

    public async Task<List<TEntity>> GetAsync(ISpecification<TEntity> specification)
    {
        var items = await ApplySpecification(specification).ToListAsync();
        return items;
    }

    public async Task<List<TEntity>> GetReadyOnlyAsync(ISpecification<TEntity> specification)
    {
        var items = await ApplySpecificationReadOnly(specification).ToListAsync();
        return items;
    }

    public async Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(ISpecification<TEntity> specification)
    {
        var items = await ApplySpecificationReadOnly(specification).ToListAsync();
        return new PaginationResult<TEntity, TEntityId>(items, await CountAsync(specification, countAllExcludingPagination: true), specification.ResourceParameter); 
    }

    public async Task<TEntity?> GetFirstOrDefault(ISpecification<TEntity> specification) 
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetFirstOrDefaultReadyOnly(ISpecification<TEntity> specification)
    {
        return await ApplySpecificationReadOnly(specification).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckExistAsync(ISpecification<TEntity> specification)
    {
        return await ApplySpecification(specification).AnyAsync();
    }

    public async Task<bool> CheckExistByIdAsync(TEntityId id)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(x => x.Id.Equals(id));
    }

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> ListAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
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

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
    }

    private IQueryable<TEntity> ApplySpecificationReadOnly(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec).AsNoTracking();
    }
}