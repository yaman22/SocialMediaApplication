using SocialMediaApp.Application.Core.CQRS;

namespace SocialMediaApp.Application.Posts.Commands.ChangeLikeStatus;

public class ChangeLikeStatusCommand
{
    public class Request : IRequest
    {
        public Guid PostId { get; init; }
    }
}