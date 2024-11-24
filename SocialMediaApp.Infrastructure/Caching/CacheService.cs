using Microsoft.Extensions.Caching.Memory;
using SocialMediaApp.Application.Core.Abstraction.Caching;

namespace SocialMediaApp.Infrastructure.Caching;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    
    
    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? TryGet<T>(object key)
    {
        return _memoryCache.TryGetValue(key,out T value) ? value : default;
    }

    public void Set<T>(object key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30))
            .SetAbsoluteExpiration(TimeSpan.FromHours(2))
            .SetPriority(CacheItemPriority.Normal)
            .SetSize(2);
            
        _memoryCache.Set(key, value, cacheEntryOptions);
    }
}