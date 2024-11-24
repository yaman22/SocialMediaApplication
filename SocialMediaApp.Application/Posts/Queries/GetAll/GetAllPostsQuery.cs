using System.Linq.Expressions;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Enums;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Shared.DTOs;

namespace SocialMediaApp.Application.Posts.Queries.GetAll;

public class GetAllPostsQuery
{
    public class Request : OffsetPagination, IRequest<Response>
    {
        public OrderType OrderType { get; set; } = Domain.Core.Enums.OrderType.Descending;
        public string? Search { get; init; }
        public Guid? UserId { get; init; }
    }

    public class Response
    {
        public int Count { get; init; }
        public List<PostResponse> Posts { get; init; } = new();

        public class PostResponse
        {
            public Guid Id { get; init; }
            public string? Content { get; init; }
            public List<string>? FileUrls { get; init; } = new();
            public DateTimeOffset DatePosted { get; init; }
            public UserResponse User { get; init; }
            public bool IsLiked { get; init; }
            public int LikesCount { get; init; }
            public int CommentsCount { get; init; }
        }

        public class UserResponse
        {
            public Guid Id { get; init; }
            public string UserName { get; init; }
            public string? ImageUrl { get; init; }
        }

        public static Expression<Func<Post, PostResponse>> Mapper(Guid currentUserId) => post
            => new()
            {
                Id = post.Id,
                Content = post.Content,
                FileUrls = post.FileUrls,
                DatePosted = post.DateCreated,
                LikesCount = post.Likes.Count(like => !like.DateDeleted.HasValue),
                CommentsCount = post.Comments.Count(comment => !comment.DateDeleted.HasValue),
                IsLiked = post.Likes.Any(like => !like.DateDeleted.HasValue && like.UserId == currentUserId),
                User = new UserResponse()
                {
                    Id = post.UserId,
                    UserName = post.User.UserName,
                    ImageUrl = post.User.ImageUrl,
                },
            };
    };
}
