using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Domain.Entities.Posts;

public class Like : Entity
{
    private Like()
    {
    }

    public Like(Guid userId, Guid postId)
    {
        UserId = userId;
        PostId = postId;
    }
    
    public Guid PostId { get; private set; }
    public Post Post { get; private set; }
    
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public void ChangeStatus()
    {
        DateDeleted = DateDeleted == null ? DateTimeOffset.UtcNow : null;
        DeletedBy = DateDeleted == null ? null : UserId;
    }
}