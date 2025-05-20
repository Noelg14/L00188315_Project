using L00188315_Project.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace L00188315_Project.Infrastructure.Services;
/// <summary>
/// Implementation of the cache service.
/// </summary>
/// <param name="_cache"></param>
public class CacheService(IMemoryCache _cache) : ICacheService
{
    /// <summary>
    /// Clear cache for a given key.
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Clear(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        }
        _cache.Remove(key);
    }
    /// <summary>
    /// Get a value from the cache.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key)
    {
        var value = _cache.Get<string>(key);
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        return value;
    }
    /// <summary>
    /// Sets a value in the cache with an expiry time.
    /// </summary>
    /// <param name="key">Key for the value</param>
    /// <param name="value">the value to be cached</param>
    /// <param name="seconds">seconds the key is to persist in the cache default 3600</param>
    /// <returns></returns>
    public string Set(string key, string value, int seconds = 3600)
    {
        TimeSpan expiry = TimeSpan.FromSeconds(seconds);
        return _cache.Set(key, value, expiry);
    }
}
