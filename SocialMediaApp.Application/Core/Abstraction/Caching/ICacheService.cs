namespace SocialMediaApp.Application.Core.Abstraction.Caching;

public interface ICacheService
{
    T? TryGet<T>(object key);
    
    void Set<T>(object key, T value);
}