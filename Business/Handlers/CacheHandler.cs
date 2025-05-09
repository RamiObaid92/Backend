﻿using Microsoft.Extensions.Caching.Memory;

namespace Business.Handlers;

public interface ICacheHandler<T>
{
    T? GetFromCache(string cacheKey);
    void RemoveCache(string cacheKey);
    T SetCache(string cacheKey, T data, int minutesToCache = 10);
}

public class CacheHandler<T>(IMemoryCache cache) : ICacheHandler<T>
{
    private readonly IMemoryCache _cache = cache;

    public T? GetFromCache(string cacheKey) 
        => _cache.Get<T>(cacheKey);

    public T SetCache(string cacheKey, T data, int minutesToCache = 10)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutesToCache)
        };
        _cache.Set(cacheKey, data, options);
        return data;
    }

    public void RemoveCache(string cacheKey) 
        => _cache.Remove(cacheKey);
}

// Tog hjälp av AI för att lära mig och skapa en CacheHandlern.