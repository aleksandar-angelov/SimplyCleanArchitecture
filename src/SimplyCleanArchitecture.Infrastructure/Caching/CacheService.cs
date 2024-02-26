using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SimplyCleanArchitecture.Application.Common.Abstractions;
using System.Collections.Concurrent;

namespace SimplyCleanArchitecture.Infrastructure.Caching;

public class CacheService : ICache
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cacheValue == null)
        {
            return null;
        }

        T value = JsonConvert.DeserializeObject<T>(cacheValue);

        return value;
    }
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var cacheValue = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, true);

    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }

}
