using System.Security.Cryptography;
using System.Text;

using Core.Application.Common.Persistence;
using Core.Application.Common.Services;
using Core.Domain.Common.Models;
using Core.Domain.Common.Persistence.Models;

using Newtonsoft.Json;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public class GenericCachingRepository<TEntity, TEntityId>(
    IGenericRepository<TEntity, TEntityId> decoratedRepository,
    ICachingService cachingService)
    : IGenericRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    private static readonly string CollectionCacheKey = typeof(TEntity).Name;
   
    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await cachingService.GetOrCreateAsync(
            OperationKey(nameof(CountAllAsync)), 
            factory: async () => (object)await decoratedRepository.CountAllAsync(cancellationToken), 
            cancellationToken);

        if (result is null)
        {
            return default;
        }

        return (int)result;
    }

    public async Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> specification,
        bool countAllExcludingPagination = false,
        CancellationToken cancellationToken = default)
    {
        var countAllExcludingPart = countAllExcludingPagination ? "-without-pagination" : string.Empty;
            
        var result = await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, $"{nameof(CountAsync)}{countAllExcludingPart}"), 
            factory: async () => (object)await decoratedRepository.CountAsync(
                specification, 
                countAllExcludingPagination, 
                cancellationToken), 
            cancellationToken);
        
        if (result is null)
        {
            return default;
        }

        return (int)result;
    }

    public async Task<PaginationResult<TEntity, TEntityId>> GetPaginationAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, nameof(GetPaginationAsync)), 
            factory: async () => await decoratedRepository.GetPaginationAsync(specification, cancellationToken), 
            cancellationToken) ?? throw new InvalidOperationException();
    }

    public async Task<List<TEntity>> GetAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, nameof(GetAsync)), 
            factory: async () => await decoratedRepository.GetAsync(specification, cancellationToken), 
            cancellationToken) ?? throw new InvalidOperationException();
    }

    public async Task<List<TEntity>> GetReadyOnlyAsync(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, nameof(GetReadyOnlyAsync)), 
            factory: async () => await decoratedRepository.GetReadyOnlyAsync(specification, cancellationToken), 
            cancellationToken) ?? throw new InvalidOperationException();
    }

    public async Task<TEntity?> GetFirstOrDefault(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, nameof(GetFirstOrDefault)),
            factory: async () => await decoratedRepository.GetFirstOrDefault(specification, cancellationToken),
            cancellationToken);
    }

    public async Task<TEntity?> GetFirstOrDefaultReadyOnly(
        ISpecification<TEntity, TEntityId> specification, 
        CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            SpecificationKey(specification, nameof(GetFirstOrDefaultReadyOnly)),
            factory: async () => await decoratedRepository.GetFirstOrDefaultReadyOnly(specification, cancellationToken),
            cancellationToken);
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
            factory: async () => await decoratedRepository.GetByIdAsync(id, cancellationToken), 
            cancellationToken);
    }

    public async Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await cachingService.GetOrCreateAsync(
            CollectionCacheKey, 
            factory: async () => await decoratedRepository.ListAllAsync(cancellationToken), 
            cancellationToken) ?? []; 
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await decoratedRepository.AddAsync(entity, cancellationToken);
        
        await cachingService.SetAsync(MemberKey(entity.Id), result, cancellationToken);
        
        return result;
    }

    public void Update(TEntity entity)
    {
        decoratedRepository.Update(entity);
        cachingService.RemoveAsync(MemberKey(entity.Id)).Wait();
        cachingService.RemoveAsync(CollectionCacheKey).Wait();
    }

    public void Delete(TEntity entity)
    {
        decoratedRepository.Delete(entity);
        cachingService.RemoveAsync(MemberKey(entity.Id)).Wait();
        cachingService.RemoveAsync(CollectionCacheKey).Wait();
    }

    public void DeleteRange(Func<TEntity, bool> predicate)
    {
        decoratedRepository.DeleteRange(predicate);
        cachingService.RemoveByPrefixAsync(CollectionCacheKey).Wait();
    }
    
    private static string MemberKey(TEntityId id) => $"{CollectionCacheKey}-{id}";
    
    private static string SpecificationKey<TSpec>(TSpec specification, string operationName)
    {
        var json = JsonConvert.SerializeObject(specification);
        var hashBytes = SHA256.HashData(source: Encoding.UTF8.GetBytes(json));
        var hash = BitConverter.ToString(hashBytes).Replace(oldValue: "-", newValue: string.Empty);
        return $"{CollectionCacheKey}-Spec-{hash}-operation-{operationName}";
    }
    
    private static string OperationKey(string operationName) => $"{CollectionCacheKey}-operation-{operationName}";
}