using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.ModifyComment;

public class ModifyCommentHandler(IRepository repository, IFileService fileService)
    : IRequestHandler<ModifyCommentCommand.Request>
{
    public async Task<Result> HandleAsync(ModifyCommentCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        if(request.IsFileDeleted && request.File is null && string.IsNullOrEmpty(request.Content))
            return Result.Failure(DomainErrors.Comment.CommentMustHaveTextOrFile());
        
        var comment = await repository.TrackingQuery<Comment>()
            .FirstAsync(comment => comment.Id == request.Id, cancellationToken);
        
        if(request.IsFileDeleted)
            fileService.Delete(comment.FileUrl);
        
        var fileUrl = await fileService.UploadAsync(request.File);

        if (!request.IsFileDeleted && string.IsNullOrEmpty(fileUrl))
            fileUrl = comment.FileUrl;

        comment.Modify(request.Content, fileUrl);
        
        repository.Update(comment);

        await repository.SaveChangeAsync(cancellationToken);
        
        return Result.Success();
    }
}