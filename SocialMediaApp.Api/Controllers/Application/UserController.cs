using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Api.Controllers.Base;
using SocialMediaApp.Api.Controllers.Base.Attributes;
using SocialMediaApp.Api.Controllers.Base.Extensions;
using SocialMediaApp.Api.Controllers.Base.Swagger;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Users.Commands.ChangePassword;
using SocialMediaApp.Application.Users.Commands.LogIn;
using SocialMediaApp.Application.Users.Commands.Modify;
using SocialMediaApp.Application.Users.Commands.SignUp;
using SocialMediaApp.Application.Users.Queries.GetProfile;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Api.Controllers.Application;

public class UserController : ApiController
{
    [Authorize]
    [HttpGet, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetUserProfileQuery.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(
        [FromServices] IRequestHandler<GetUserProfileQuery.Request, GetUserProfileQuery.Response> handler)
        => await handler.HandleAsync(new()).ToJsonResultAsync();
    
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(LogInUserCommand.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromBody] LogInUserCommand.Request request,
        [FromServices] IRequestHandler<LogInUserCommand.Request, LogInUserCommand.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(SignUpUserCommand.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignUp(
        [FromForm] SignUpUserCommand.Request request,
        [FromServices] IRequestHandler<SignUpUserCommand.Request, SignUpUserCommand.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(GetUserProfileQuery.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Modify(
        [FromForm] ModifyUserCommand.Request request,
        [FromServices] IRequestHandler<ModifyUserCommand.Request, GetUserProfileQuery.Response> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
    
    [Authorize]
    [HttpPost, SocialMediaAppRoute(RouteType.Application), ApiGroup(RouteType.Application)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangeUserPasswordCommand.Request request,
        [FromServices] IRequestHandler<ChangeUserPasswordCommand.Request> handler)
        => await handler.HandleAsync(request).ToJsonResultAsync();
}