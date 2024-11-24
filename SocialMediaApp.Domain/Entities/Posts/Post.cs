using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Domain.Entities.Posts;

public class Post : Entity
{
    private Post()
    {
    }

    public Post(Guid userId, string? content, List<string>? fileUrls)
    {
        UserId = userId;
        Content = content ?? string.Empty;
        FileUrls = fileUrls ?? new List<string>();
    }

    public string Content { get; private set; } = string.Empty;
    public List<string>? FileUrls { get; private set; } = new();


    public Guid UserId { get; private set; }
    public User User { get; private set; }

    private readonly List<Comment> _comments = new List<Comment>();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private readonly List<Like> _likes = new List<Like>();
    public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

    public void Modify(string? content, List<string>? fileUrls, List<string>? deletedFileUrls)
    {
        Content = content ?? string.Empty;
        
        if (deletedFileUrls is not null)
        {
            FileUrls.RemoveAll(deletedFileUrls.Contains);
        }

        if (fileUrls is not null)
        {
            FileUrls.AddRange(fileUrls);
        }
    }
}