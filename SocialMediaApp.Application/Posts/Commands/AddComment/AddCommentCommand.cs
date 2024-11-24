using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAllComments;

namespace SocialMediaApp.Application.Posts.Commands.AddComment;

public class AddCommentCommand
{
    public class Request : IRequest<GetAllCommentsQuery.Response.CommentResponse>
    {
        public Guid PostId { get; init; }
        public Guid? BaseCommentId { get; init; }
        public string? Content { get; init; }
        public IFormFile? File { get; init; }
    }
}