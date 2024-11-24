using Microsoft.AspNetCore.Identity;

namespace SocialMediaApp.Domain.Core.Primitives;

public class UserEntity : IdentityUser<Guid>, IUserEntity<Guid>, IBaseEntity
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? DeletedBy { get; set; }
    public Guid BlockedBy { get; set; }
    public DateTimeOffset? DateBlocked { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
    public bool IsBlocked() => DateBlocked.HasValue;
    public bool IsDeleted() => DateDeleted.HasValue;
}