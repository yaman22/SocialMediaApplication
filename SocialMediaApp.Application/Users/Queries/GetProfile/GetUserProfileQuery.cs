using System.Linq.Expressions;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Entities.Users;
using SocialMediaApp.Domain.Enums;

namespace SocialMediaApp.Application.Users.Queries.GetProfile;

public class GetUserProfileQuery
{
    public class Request : IRequest<Response>
    {
        public Guid? Id { get; init; }
    }
    
    public class Response
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public Gender Gender { get; init; }
        public DateTime BirthDate { get; init; }
        public string? ImageUrl { get; init; }
        public string? Bio { get; init; }
        public int PostsCount { get; init; }


        public static Expression<Func<User, Response>> Mapper() => user
            => new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                ImageUrl = user.ImageUrl,
                Bio = user.Bio,
                PostsCount = user.Posts.Count(post=>!post.DateDeleted.HasValue),
            };
    }
}