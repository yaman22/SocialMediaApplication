using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.ChangeLikeStatus;

public class ChangeLikeStatusHandler(IRepository repository, IHttpService httpService)
    : IRequestHandler<ChangeLikeStatusCommand.Request>
{
    public async Task<Result> HandleAsync(ChangeLikeStatusCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        var like = await repository.TrackingQuery<Like>().FirstOrDefaultAsync(
            like => like.PostId == request.PostId && like.UserId == httpService.GetCurrentUserId().Value,
            cancellationToken);

        if (like is not null)
        {
            like.ChangeStatus();
            
            repository.Update(like);
        }
        else
        {
            like = new Like(httpService.GetCurrentUserId().Value, request.PostId);
            
            repository.Add(like);
        }

        await repository.SaveChangeAsync(cancellationToken);
        
        return Result.Success();
    }
}