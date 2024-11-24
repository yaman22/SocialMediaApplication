using SocialMediaApp.Application.Core.Abstraction.Caching;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Application.Users.Queries.GetProfile;

public class GetUserProfileHandler(IRepository repository, IHttpService httpService, ICacheService cacheService)
    : IRequestHandler<GetUserProfileQuery.Request, GetUserProfileQuery.Response>
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task<Result<GetUserProfileQuery.Response>> HandleAsync(GetUserProfileQuery.Request request,
        CancellationToken cancellationToken = default)
    {
        var userProfile =
            cacheService.TryGet<GetUserProfileQuery.Response>(request.Id ?? httpService.GetCurrentUserId().Value);

        if (userProfile is not null)
        {
            return userProfile;
        }

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            
            userProfile =
                cacheService.TryGet<GetUserProfileQuery.Response>(request.Id ?? httpService.GetCurrentUserId().Value);

            if (userProfile is not null)
            {
                return userProfile;
            }
            else
            {
                userProfile =  await repository.QueryAsync(request.Id ?? httpService.GetCurrentUserId().Value,
                    GetUserProfileQuery.Response.Mapper(), cancellationToken);
                
                cacheService.Set(request.Id ?? httpService.GetCurrentUserId(), userProfile);
                
                return userProfile;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}