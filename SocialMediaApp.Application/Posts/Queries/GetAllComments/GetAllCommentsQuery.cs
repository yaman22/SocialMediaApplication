using System.Linq.Expressions;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Shared.DTOs;

namespace SocialMediaApp.Application.Posts.Queries.GetAllComments;

public class GetAllCommentsQuery
{
    public class Request : OffsetPagination,IRequest<Response>
    {
        public Guid? PostId { get; init; }
        public Guid? BaseCommentId { get; init; }
    }
    
    public class Response
    {
        public int Count { get; init; }
        public List<CommentResponse> Comments { get; init; } = new();
        
        public class CommentResponse
        {
            public Guid Id { get; init; }
            public Guid PostId { get; init; }
            public DateTimeOffset DateCreated { get; init; }
            public string Content { get; init; }
            public string? FileUrls { get; init; }
            public bool HasSubComments { get; init; }
            public UserResponse User { get; init; }
        }
        
        public class UserResponse
        {
            public Guid Id { get; init; }
            public string UserName { get; init; }
            public string? ImageUrl { get; init; }
        }

        public static Expression<Func<Comment, CommentResponse>> Mapper() => comment
            => new()
            {
                Id = comment.Id,
                PostId = comment.PostId,
                DateCreated = comment.DateCreated,
                Content = comment.Content,
                FileUrls = comment.FileUrl,
                HasSubComments = comment.Replies.Any(subComment=>!subComment.DateDeleted.HasValue),
                User = new UserResponse()
                {
                    Id = comment.UserId,
                    UserName = comment.User.UserName,
                    ImageUrl = comment.User.ImageUrl,
                }
            };
    }
}