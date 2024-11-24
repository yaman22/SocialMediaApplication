using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Entities.Users;
using Microsoft.IdentityModel.Tokens;

namespace SocialMediaApp.Domain.Entities.Posts;

public class Comment : Entity
{
    private Comment()
    {
    }

    public Comment(Guid userId, Guid postId, string? content, string? fileUrl, Guid? baseCommentId)
    {
        UserId = userId;
        PostId = postId;
        Content = content;
        FileUrl = fileUrl;
        BaseCommentId = baseCommentId;
    }
    
    public string Content { get; private set; } = string.Empty;
    public string? FileUrl { get; private set; }

    public Guid? BaseCommentId { get; private set; }
    public Comment? BaseComment { get; private set; }

    private readonly List<Comment> _replies = new List<Comment>();
    public IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();
    
    public Guid PostId { get; private set; }
    public Post Post { get; private set; }
    
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public void Modify(string? content, string? fileUrl)
    {
        Content = content;
        FileUrl = fileUrl;
    }
}