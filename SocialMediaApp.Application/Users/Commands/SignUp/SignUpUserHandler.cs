using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.Abstraction.Security;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Core.Results.Extensions;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Application.Users.Commands.SignUp;

public class SignUpUserHandler(
    UserManager<User> userManager,
    IJwtBearerGenerator jwtBearerGenerator,
    IRepository repository,
    IFileService fileService) : IRequestHandler<SignUpUserCommand.Request, SignUpUserCommand.Response>
{
    public async Task<Result<SignUpUserCommand.Response>> HandleAsync(SignUpUserCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        if (await repository.Query<User>()
                .AnyAsync(user => user.Email.ToUpper() == request.Email.ToUpper(), cancellationToken))
        {
            return Result.Failure<SignUpUserCommand.Response>(DomainErrors.User.EmailAlreadyExist(request.Email));
        }

        var imageUrl = await fileService.UploadAsync(request.Image);

        var user = new User(request.FirstName, request.LastName, request.UserName, imageUrl, request.Gender,
            request.BirthDate, request.Email, request.PhoneNumber, request.Bio);
        
        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (!identityResult.Succeeded)
        {
            return identityResult.ToResult<SignUpUserCommand.Response>();
        }
     
        var accessToken = await jwtBearerGenerator.GenerateToken(user);

        return await repository.QueryAsync
            (user.Id, SignUpUserCommand.Response.Mapper(accessToken),cancellationToken);
    }
}