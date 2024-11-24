using SocialMediaApp.Application.Core.CQRS;

namespace SocialMediaApp.Application.Posts.Commands.Delete;

public class DeletePostCommand
{
    public class Request : IRequest
    {
        public Guid Id { get; init; }
    }
}