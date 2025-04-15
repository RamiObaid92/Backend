using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public interface ICacheService
{
    bool Exists(string key);
    Task<bool> ExistsAsync(string key);
    T? Get<T>(string key);
    Task<T?> GetAsync<T>(string key);
    void Remove(string key);
    Task RemoveAsync(string key);
    void Set<T>(string key, T value, TimeSpan? expiration = null);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
}

// Tog hjälp av AI för att lära mig och implementera en cachetjänst, som därefter använder Decorator-mönstret för att skapa en cachad version av en tjänst.
public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);

    public T? Get<T>(string key) => _memoryCache.TryGetValue(key, out T? value) ? value : default;

    public Task<T?> GetAsync<T>(string key) => Task.FromResult(Get<T>(key));

    public void Set<T>(string key, T value, TimeSpan? expiration = null) => _memoryCache.Set(key, value, expiration ?? _cacheDuration);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        Set(key, value, expiration);
        return Task.CompletedTask;
    }

    public void Remove(string key) => _memoryCache.Remove(key);

    public Task RemoveAsync(string key)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public bool Exists(string key) => _memoryCache.TryGetValue(key, out _);

    public Task<bool> ExistsAsync(string key) => Task.FromResult(Exists(key));
}
