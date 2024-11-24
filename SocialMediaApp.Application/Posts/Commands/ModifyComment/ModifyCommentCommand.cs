using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;

namespace SocialMediaApp.Application.Posts.Commands.ModifyComment;

public class ModifyCommentCommand
{
    public class Request : IRequest
    {
        public Guid Id { get; init; }
        public string? Content { get; init; }
        public IFormFile? File { get; init; }
        public bool IsFileDeleted { get; init; }
    }
}