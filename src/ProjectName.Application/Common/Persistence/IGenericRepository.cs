using ProjectName.Domain.Common.Models;
using ProjectName.Domain.Common.Persistence.Models;

namespace ProjectName.Application.Common.Persistence;

public interface IGenericRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    Task<int> CountAllAsync(CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> specification, 
        bool countAllExcludingPagination = false, 
        CancellationToken cancellationToken = default);
    
    Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> GetAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> GetReadyOnlyAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetFirstOrDefault(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetFirstOrDefaultReadyOnly(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<bool> CheckExistAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default);
    
    Task<bool> CheckExistByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);
    
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    void Update(TEntity entity);
    
    void Delete(TEntity entity);
    
    void DeleteRange(Func<TEntity, bool> predicate);
}