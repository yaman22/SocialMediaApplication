using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Enums;

namespace SocialMediaApp.Domain.Entities.Users;

public class User : UserEntity
{
    private User()
    {
    }

    public User(string firstName, string lastName, string? userName, string? imageUrl, Gender gender,
        DateTime birthDate, string email, string phoneNumber, string? bio)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        BirthDate = birthDate;
        Email = email;
        PhoneNumber = phoneNumber;
        ImageUrl = imageUrl;
        Bio = bio;

        if (string.IsNullOrEmpty(userName))
        {
            UserName = firstName + lastName;
        }
        else
        {
            UserName = userName.Trim();
        }
    }

    public string? ImageUrl { get; private set; }
    public string? Bio { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime BirthDate { get; private set; }

    private readonly List<Post> _posts = new List<Post>();
    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

    private readonly List<Comment> _comments = new List<Comment>();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private readonly List<Like> _likes = new List<Like>();
    public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

    public void Modify(string firstName, string lastName, string email, string? imageUrl,
        string phoneNumber, Gender gender, string userName, DateTime birthDate,
        string? bio)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        BirthDate = birthDate;
        Email = email;
        PhoneNumber = phoneNumber;
        ImageUrl = imageUrl;
        Bio = bio;
        UserName = userName.Trim();
    }
}