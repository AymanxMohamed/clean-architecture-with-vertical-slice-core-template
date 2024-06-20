using System.Security.Cryptography;
using System.Text;

using Core.Application.Common.Persistence;
using Core.Application.Common.Services;
using Core.Domain.Common.Models;
using Core.Domain.Common.Persistence.Models;

using Newtonsoft.Json;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class CachedGenericRepository<TEntity, TEntityId>(
    IGenericRepository<TEntity, TEntityId> genericRepository,
    ICachingService cachingService)
    : ICachedGenericRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    private static readonly string CollectionCacheKey = typeof(TEntity).Name;
   
    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        return await genericRepository.CountAllAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> specification,
        bool countAllExcludingPagination = false,
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.CountAsync(specification, countAllExcludingPagination, cancellationToken);
    }

    public async Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.GetPaginationAsync(specification, cancellationToken);
    }

    public async Task<List<TEntity>> GetAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.GetAsync(specification, cancellationToken);
    }

    public async Task<List<TEntity>> GetReadyOnlyAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.GetReadyOnlyAsync(specification, cancellationToken);
    }

    public async Task<TEntity?> GetFirstOrDefault(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.GetFirstOrDefault(specification, cancellationToken);
    }

    public async Task<TEntity?> GetFirstOrDefaultReadyOnly(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await genericRepository.GetFirstOrDefaultReadyOnly(specification, cancellationToken);
    }

    public async Task<bool> CheckExistAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await GetFirstOrDefault(specification, cancellationToken) is not null;
    }

    public async Task<bool> CheckExistByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken) is not null;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            MemberKey(id), 
            factory: async () => await genericRepository.GetByIdAsync(id, cancellationToken), 
            cancellationToken);
    }

    public async Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            CollectionCacheKey, 
            factory: async () => await genericRepository.ListAllAsync(cancellationToken), 
            cancellationToken) ?? []; 
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await genericRepository.AddAsync(entity, cancellationToken);
        
        await cachingService.SetAsync(MemberKey(entity.Id), result, cancellationToken);
        await cachingService.RemoveAsync(CollectionCacheKey, cancellationToken);
        
        return result;
    }

    public void Update(TEntity entity)
    {
        genericRepository.Update(entity);
        cachingService.RemoveAsync(MemberKey(entity.Id)).Wait();
        cachingService.RemoveAsync(CollectionCacheKey).Wait();
    }

    public void Delete(TEntity entity)
    {
        genericRepository.Delete(entity);
        cachingService.RemoveAsync(MemberKey(entity.Id)).Wait();
        cachingService.RemoveAsync(CollectionCacheKey).Wait();
    }

    public void DeleteRange(Func<TEntity, bool> predicate)
    {
        genericRepository.DeleteRange(predicate);
        cachingService.RemoveByPrefixAsync(CollectionCacheKey).Wait();
    }
    
    private static string MemberKey(TEntityId id) => $"{CollectionCacheKey}-{id}";
    
    private static string SpecificationKey<TSpec>(TSpec specification, string operationName)
    {
        // todo: we need to handle the circular dependency while serializing the specification
        var json = JsonConvert.SerializeObject(specification);
        var hashBytes = SHA256.HashData(source: Encoding.UTF8.GetBytes(json));
        var hash = BitConverter.ToString(hashBytes).Replace(oldValue: "-", newValue: string.Empty);
        return $"{CollectionCacheKey}-Spec-{hash}-operation-{operationName}";
    }
    
    private static string OperationKey(string operationName) => $"{CollectionCacheKey}-operation-{operationName}";
}