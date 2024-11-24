using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Core.Results.Extensions;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Application.Users.Commands.ChangePassword;

public class ChangeUserPasswordHandler(IRepository repository, UserManager<User> userManager, IHttpService httpService) : IRequestHandler<ChangeUserPasswordCommand.Request>
{
    public async Task<Result> HandleAsync(ChangeUserPasswordCommand.Request request, CancellationToken cancellationToken = default)
    {
        var user = await repository.TrackingQuery<User>()
            .FirstAsync(user => user.Id == httpService.GetCurrentUserId().Value, cancellationToken);
        
        var identityResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!identityResult.Succeeded)
        {
            return identityResult.ToResult<Result>();
        }
        
        return Result.Success();
    }
}