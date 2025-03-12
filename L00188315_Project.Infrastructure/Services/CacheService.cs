using L00188315_Project.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;


namespace L00188315_Project.Infrastructure.Services;
public class CacheService(IMemoryCache _cache) : ICacheService
{
    public void Clear(string key)
    {
        _cache.Remove(key);
    }
    public Task<T?> Get<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }
    public void Set<T>(string key, T value,int seconds = 3600)
    {
        TimeSpan expiry = TimeSpan.FromSeconds(seconds);
        _cache.Set(key, value, expiry);
    }
}

