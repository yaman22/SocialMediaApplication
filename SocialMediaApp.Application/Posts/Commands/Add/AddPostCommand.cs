using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAll;

namespace SocialMediaApp.Application.Posts.Commands.Add;

public class AddPostCommand
{
    public class Request : IRequest<GetAllPostsQuery.Response.PostResponse>
    {
        public string? Content { get; init; }
        public List<IFormFile>? Files { get; init; } = new();
    }
}