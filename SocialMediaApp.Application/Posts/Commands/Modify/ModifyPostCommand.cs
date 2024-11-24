using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAll;

namespace SocialMediaApp.Application.Posts.Commands.Modify;

public class ModifyPostCommand
{
    public class Request : IRequest<GetAllPostsQuery.Response.PostResponse>
    {
        public Guid Id { get; init; }
        public string? Content { get; init; }
        public List<string>? DeletedFileUrls { get; init; } = new();
        public List<IFormFile>? Files { get; init; } = new();
    }
}