using L00188315_Project.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;


namespace L00188315_Project.Infrastructure.Services;
public class CacheService(IMemoryCache _cache) : ICacheService
{
    public void Clear(string key)
    {
        //_cache.Remove(key);
    }
    public string Get(string key)
    {
        var value = _cache.Get<string>(key);
        if (string.IsNullOrEmpty(value)){
            return string.Empty;
        }
        return value;
    }
    public string Set(string key, string value,int seconds = 3600)
    {
        TimeSpan expiry = TimeSpan.FromSeconds(seconds);
        return _cache.Set(key, value, expiry);
    }
}

