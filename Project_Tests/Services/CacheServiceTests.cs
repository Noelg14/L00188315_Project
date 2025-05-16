using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Project_Tests;

/// <summary>
/// Tests for the CacheService class.
/// </summary>
public class CacheServiceTests
{
    [Fact]
    /// <summary>
    /// Sets up the test class with the necessary dependencies.
    /// </summary>
    public void Cache_Can_Set_Value()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var expiration = TimeSpan.FromSeconds(60);
        var _cacheService = new CacheService(GetSystemUnderTest());
        // Act
        var result = _cacheService.Set(key, value, 60);
        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Cache_Can_Get_Value()
    {
        // Arrange
        var key = "testKey";
        var value = "testValue";
        var inMemoryCache = GetSystemUnderTest();
        var _cacheService = new CacheService(inMemoryCache);
        inMemoryCache.Set(key, value, TimeSpan.FromSeconds(60)); // Set in the Cache Directly for test
        // Act
        var result = _cacheService.Get(key);
        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Cache_Can_Clear_Value()
    {
        // Arrange
        var key = "testKey";
        var _cacheService = new CacheService(GetSystemUnderTest());
        // Act
        _cacheService.Clear(key);
        // Assert
        _cacheService.Get(key);
        Assert.Equal(string.Empty, _cacheService.Get(key));
    }

    [Fact]
    public void Cache_Get_Returns_Empty_String_When_Value_Not_Found()
    {
        // Arrange
        var key = "nonExistentKey";
        var _cacheService = new CacheService(GetSystemUnderTest());
        // Act
        var result = _cacheService.Get(key);
        // Assert
        Assert.Equal(string.Empty, result);
    }

    public IMemoryCache GetSystemUnderTest()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        return memoryCache!;
    }
}
