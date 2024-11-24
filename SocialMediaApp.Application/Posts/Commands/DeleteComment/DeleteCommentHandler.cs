using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.DeleteComment;

public class DeleteCommentHandler(IRepository repository, IFileService fileService)
    : IRequestHandler<DeleteCommentCommand.Request>
{
    public async Task<Result> HandleAsync(DeleteCommentCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        var comment = await repository.TrackingQuery<Comment>()
            .FirstAsync(comment => comment.Id == request.Id, cancellationToken);
        
        repository.Remove(comment);

        await repository.SaveChangeAsync(cancellationToken);

        fileService.Delete(comment.FileUrl);
        
        return Result.Success();
    }
}