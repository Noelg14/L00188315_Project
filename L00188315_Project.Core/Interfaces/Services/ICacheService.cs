namespace L00188315_Project.Core.Interfaces.Services;
/// <summary>
/// Interface for the cache service. Allows for changing the cache service implementation without changing the code that uses it.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Get a value from the cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<T> Get<T>(string key);
    /// <summary>
    /// Set a value in the cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void Set<T>(string key,T value, int? seconds);
    /// <summary>
    /// Clear a value from the cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public void Clear(string key);
}
