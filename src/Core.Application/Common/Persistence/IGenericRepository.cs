using Core.Domain.Common.Persistence.Models;
using Core.Domain.Models;

namespace Core.Application.Common.Persistence;

public interface IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    Task<int> CountAllAsync();
    Task<int> CountAsync(ISpecification<TEntity, TEntityId> specification, bool countAllExcludingPagination = false);
    Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(ISpecification<TEntity, TEntityId> specification);
    Task<List<TEntity>> GetAsync(ISpecification<TEntity, TEntityId> specification);
    Task<List<TEntity>> GetReadyOnlyAsync(ISpecification<TEntity, TEntityId> specification);
    Task<TEntity?> GetFirstOrDefault(ISpecification<TEntity, TEntityId> specification);
    Task<TEntity?> GetFirstOrDefaultReadyOnly(ISpecification<TEntity, TEntityId> specification);
    Task<bool> CheckExistAsync(ISpecification<TEntity, TEntityId> specification);
    Task<bool> CheckExistByIdAsync(TEntityId id);
    Task<TEntity?> GetByIdAsync(TEntityId id);
    Task<List<TEntity>> ListAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteRange(Func<TEntity, bool> predicate);
}