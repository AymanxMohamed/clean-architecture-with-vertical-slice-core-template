using Core.Domain.Common.Persistence.Models;
using Core.Domain.Models;

namespace Core.Application.Common.Persistence;

public interface IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    Task<int> CountAllAsync();
    Task<int> CountAsync(ISpecification<TEntity> specification, bool countAllExcludingPagination = false);
    Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(ISpecification<TEntity> specification);
    Task<List<TEntity>> GetAsync(ISpecification<TEntity> specification);
    Task<List<TEntity>> GetReadyOnlyAsync(ISpecification<TEntity> specification);
    Task<TEntity?> GetFirstOrDefault(ISpecification<TEntity> specification);
    Task<TEntity?> GetFirstOrDefaultReadyOnly(ISpecification<TEntity> specification);
    Task<bool> CheckExistAsync(ISpecification<TEntity> specification);
    Task<bool> CheckExistByIdAsync(TEntityId id);
    Task<TEntity?> GetByIdAsync(TEntityId id);
    Task<List<TEntity>> ListAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteRange(Func<TEntity, bool> predicate);
}