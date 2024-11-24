using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Api.Controllers.Base;
using SocialMediaApp.Api.Controllers.Base.Attributes;
using SocialMediaApp.Api.Controllers.Base.Extensions;
using SocialMediaApp.Api.Controllers.Base.Swagger;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Commands.Add;
using SocialMediaApp.Application.Posts.Commands.AddComment;
using SocialMediaApp.Application.Posts.Commands.ChangeLikeStatus;
using SocialMediaApp.Application.Posts.Commands.Delete;
using SocialMediaApp.Application.Posts.Commands.DeleteComment;
using SocialMediaApp.Application.Posts.Commands.Modify;
using SocialMediaApp.Application.Posts.Commands.ModifyComment;
using SocialMediaApp.Application.Posts.Queries.GetAll;
using SocialMediaApp.Application.Posts.Queries.GetAllComments;
using SocialMediaApp.Application.Posts.Queries.GetAllLikes;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Api.Controllers.Application;

public class PostController : ApiController
{
    [Authorize]
    [HttpGet, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllPostsQuery.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllPostsQuery.Request request,
        [FromServices] IRequestHandler<GetAllPostsQuery.Request, GetAllPostsQuery.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpGet, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllCommentsQuery.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllComments(
        [FromQuery] GetAllCommentsQuery.Request request,
        [FromServices] IRequestHandler<GetAllCommentsQuery.Request, GetAllCommentsQuery.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpGet, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllLikesQuery.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLikes(
        [FromQuery] GetAllLikesQuery.Request request,
        [FromServices] IRequestHandler<GetAllLikesQuery.Request, GetAllLikesQuery.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllPostsQuery.Response.PostResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(
        [FromForm] AddPostCommand.Request request,
        [FromServices] IRequestHandler<AddPostCommand.Request, GetAllPostsQuery.Response.PostResponse> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllPostsQuery.Response.PostResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Modify(
        [FromForm] ModifyPostCommand.Request request,
        [FromServices] IRequestHandler<ModifyPostCommand.Request, GetAllPostsQuery.Response.PostResponse> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpDelete, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(
        [FromBody] DeletePostCommand.Request request,
        [FromServices] IRequestHandler<DeletePostCommand.Request> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetAllCommentsQuery.Response.CommentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddComment(
        [FromForm] AddCommentCommand.Request request,
        [FromServices] IRequestHandler<AddCommentCommand.Request, GetAllCommentsQuery.Response.CommentResponse> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ModifyComment(
        [FromForm] ModifyCommentCommand.Request request,
        [FromServices] IRequestHandler<ModifyCommentCommand.Request> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpDelete, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteComment(
        [FromBody] DeleteCommentCommand.Request request,
        [FromServices] IRequestHandler<DeleteCommentCommand.Request> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeLikeStatus(
        [FromBody] ChangeLikeStatusCommand.Request request,
        [FromServices] IRequestHandler<ChangeLikeStatusCommand.Request> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
}