using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Security;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Application.Users.Commands.LogIn;

public class LogInUserHandler(
    IRepository repository,
    IJwtBearerGenerator jwtBearerGenerator,
    UserManager<User> userManager) : IRequestHandler<LogInUserCommand.Request, LogInUserCommand.Response>
{
    public async Task<Result<LogInUserCommand.Response>> HandleAsync(LogInUserCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        var user = await repository
            .Query<User>()
            .FirstOrDefaultAsync(user => user.Email.ToUpper() == request.Email.ToUpper(), cancellationToken);

        if (user is null)
            return Result.Failure<LogInUserCommand.Response>(DomainErrors.User.NotFound());

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            return Result.Failure<LogInUserCommand.Response>(DomainErrors.User.EmailPasswordMismatch());

        var accessToken = await jwtBearerGenerator.GenerateToken(user);

        return await repository.QueryAsync(user.Id, LogInUserCommand.Response.Mapper(accessToken), cancellationToken);
    }
}