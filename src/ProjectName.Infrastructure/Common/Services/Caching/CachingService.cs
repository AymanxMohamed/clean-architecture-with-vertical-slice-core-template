﻿using System.Collections.Concurrent;

using ProjectName.Application.Common.Services;
using ProjectName.Infrastructure.Common.Services.Serialization.Resolvers;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace ProjectName.Infrastructure.Common.Services.Caching;

public class CachingService(IDistributedCache distributedCache, CachingSettings cachingSettings) : ICachingService
{
    private const string CacheKeysKey = "CacheKeys";
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) 
        where T : class
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        return cachedValue is null ? null : JsonConvert.DeserializeObject<T>(cachedValue, new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateResolver()
        });
    }

    public async Task<T?> GetOrCreateAsync<T>(
        string key, 
        Func<Task<T?>> factory, 
        CancellationToken cancellationToken = default) 
        where T : class
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        if (cachedValue is null)
        {
            return null;
        }

        await SetAsync(key, cachedValue, cancellationToken);

        return cachedValue;
    }
    
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) 
        where T : class
    {
        await distributedCache.SetStringAsync(
            key, 
            value: JsonConvert.SerializeObject(value), 
            options: new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cachingSettings.DefaultCacheExpiringTimeInMinutes)
            }, 
            token: cancellationToken);

        await AddCacheKey(key, cancellationToken);
    }

    Task ICachingService.SetAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        return SetAsync(key, value, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
        await RemoveCacheKey(key, cancellationToken);
    }

    public async Task RemoveByPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default)
    {
        // todo: implement better way for this method
        var removeKeysTasks = (await GetCacheKeys(cancellationToken))
            .Keys
            .Where(key => key.StartsWith(keyPrefix))
            .Select(key => RemoveAsync(key, cancellationToken));

        await Task.WhenAll(removeKeysTasks);
    }
    
    public async Task<ConcurrentDictionary<string, bool>> GetCacheKeys(CancellationToken cancellationToken = default)
    {
        return await GetAsync<ConcurrentDictionary<string, bool>>(CacheKeysKey, cancellationToken) ?? [];
    }

    private async Task AddCacheKey(string key, CancellationToken cancellationToken = default)
    {
        var cacheKeys = await GetCacheKeys(cancellationToken);
        
        if (cacheKeys.TryAdd(key, false))
        {
            await UpdateCacheKeys(cacheKeys, cancellationToken);
        }
    }
    
    private async Task RemoveCacheKey(string key, CancellationToken cancellationToken = default)
    {
        var cacheKeys = await GetCacheKeys(cancellationToken);
        
        if (cacheKeys.TryRemove(key, out _))
        {
            await UpdateCacheKeys(cacheKeys, cancellationToken);
        }
    }

    private async Task UpdateCacheKeys(
        ConcurrentDictionary<string, bool> cacheKeys, 
        CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(
            key: CacheKeysKey, 
            value: JsonConvert.SerializeObject(cacheKeys), 
            token: cancellationToken);
    }
}