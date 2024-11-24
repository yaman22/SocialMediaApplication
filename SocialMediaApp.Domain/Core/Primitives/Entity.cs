namespace SocialMediaApp.Domain.Core.Primitives;

public abstract class Entity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? DeletedBy { get; set; }
    public void SoftDelete() => DateDeleted = DateTimeOffset.UtcNow;
}