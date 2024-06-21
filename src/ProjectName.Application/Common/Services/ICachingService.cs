using System.Collections.Concurrent;

namespace ProjectName.Application.Common.Services;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;
    
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        where T : class;
    
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;
    
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default);

    Task<ConcurrentDictionary<string, bool>> GetCacheKeys(CancellationToken cancellationToken = default);
}