using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.Delete;

public class DeletePostHandler(IRepository repository) : IRequestHandler<DeletePostCommand.Request>
{
    public async Task<Result> HandleAsync(DeletePostCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        var post = await repository.TrackingQuery<Post>().FirstAsync(post => post.Id == request.Id, cancellationToken);
        
        repository.SoftDelete(post);
        
        repository.Update(post);
        
        await repository.SaveChangeAsync(cancellationToken);
        
        return Result.Success();
    }
}