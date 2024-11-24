using SocialMediaApp.Application.Core.CQRS;

namespace SocialMediaApp.Application.Posts.Commands.DeleteComment;

public class DeleteCommentCommand
{
    public class Request : IRequest
    {
        public Guid Id { get; init; }
    }
}