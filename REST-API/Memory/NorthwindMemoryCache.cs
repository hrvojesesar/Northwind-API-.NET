using Microsoft.Extensions.Caching.Memory;

namespace REST_API.Memory;

/// <summary>
/// Provides caching functionality for the Northwind application.
/// </summary>
public class NorthwindMemoryCache
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="NorthwindMemoryCache"/> class.
    /// </summary>
    /// <param name="cache">The memory cache instance.</param>
    public NorthwindMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves data from the cache. If the data is not present, it sets the data in the cache.
    /// </summary>
    /// <returns>The cached data as a string.</returns>
    public string GetData()
    {
        if (!_cache.TryGetValue("key", out string cachedData))
        {
            // Set cache
            _cache.Set("key", "This is cached data", TimeSpan.FromMinutes(5));
            cachedData = "This is cached data";
        }
        return cachedData;
    }
}