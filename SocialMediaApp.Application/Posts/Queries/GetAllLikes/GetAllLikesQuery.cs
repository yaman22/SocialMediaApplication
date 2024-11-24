using System.Linq.Expressions;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Shared.DTOs;

namespace SocialMediaApp.Application.Posts.Queries.GetAllLikes;

public class GetAllLikesQuery
{
    public class Request : OffsetPagination,IRequest<Response>
    {
        public Guid PostId { get; init; }
    }
    
    public class Response
    {
        public int Count { get; init; }
        public List<LikeResponse> Likes { get; init; } = new();
        
        public class LikeResponse
        {
            public Guid Id { get; init; }
            public DateTimeOffset DateCreated { get; init; }
            public UserResponse User { get; init; }
        }
        
        public class UserResponse
        {
            public Guid Id { get; init; }
            public string UserName { get; init; }
            public string? ImageUrl { get; init; }
        }

        public static Expression<Func<Like, LikeResponse>> Mapper() => like
            => new()
            {
                Id = like.Id,
                DateCreated = like.DateCreated,
                User = new UserResponse()
                {
                    Id = like.UserId,
                    UserName = like.User.UserName,
                    ImageUrl = like.User.ImageUrl,
                }
            };
    }
}