namespace SocialMediaApp.Domain.Core.Primitives;

public interface IUserEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey? BlockedBy { get; set; }
    public DateTimeOffset? DateBlocked { get; set; }
}