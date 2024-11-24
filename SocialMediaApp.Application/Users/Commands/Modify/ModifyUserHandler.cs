using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Caching;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Users.Queries.GetProfile;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Application.Users.Commands.Modify;

public class ModifyUserHandler(IRepository repository, IFileService fileService, IHttpService httpService, ICacheService cacheService)
    : IRequestHandler<ModifyUserCommand.Request, GetUserProfileQuery.Response>
{
    public async Task<Result<GetUserProfileQuery.Response>> HandleAsync(ModifyUserCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        if (await repository.Query<User>().AnyAsync(user=>user.Id != httpService.GetCurrentUserId().Value && user.Email == request.Email, cancellationToken: cancellationToken))
            return Result.Failure<GetUserProfileQuery.Response>(DomainErrors.User.EmailAlreadyExist(request.Email));

        var user = await repository.TrackingQuery<User>()
            .FirstAsync(user => user.Id == httpService.GetCurrentUserId().Value, cancellationToken);

        var imageUrl = await fileService.Modify(user.ImageUrl, request.Image);
        
        user.Modify(request.FirstName, request.LastName, request.Email, imageUrl, request.PhoneNumber, request.Gender,
            request.UserName, request.BirthDate, request.Bio);

        repository.Update(user);

        await repository.SaveChangeAsync(cancellationToken);

        cacheService.Set(httpService.GetCurrentUserId().Value, new GetUserProfileQuery.Response()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            UserName = user.UserName,
            Bio = user.Bio,
            ImageUrl = imageUrl,
        });
        
        return await repository.QueryAsync(user.Id, GetUserProfileQuery.Response.Mapper(), cancellationToken);
    }
}