using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAll;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.Modify;

public class ModifyPostHandler(IRepository repository, IFileService fileService)
    : IRequestHandler<ModifyPostCommand.Request, GetAllPostsQuery.Response.PostResponse>
{
    public async Task<Result<GetAllPostsQuery.Response.PostResponse>> HandleAsync(ModifyPostCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        var post = await repository.TrackingQuery<Post>().FirstAsync(post => post.Id == request.Id, cancellationToken);

        if
        (
            (
                post.FileUrls.Count == 0
                && string.IsNullOrEmpty(request.Content)
                && request.Files.Count == 0
            )
            ||
            (
                string.IsNullOrEmpty(post.Content)
                && string.IsNullOrEmpty(request.Content)
                && request.Files.Count == 0
            )
            ||
            (
                post.FileUrls.Count == request.DeletedFileUrls.Count
                && string.IsNullOrEmpty(post.Content)
                && string.IsNullOrEmpty(request.Content)
                && request.Files.Count == 0)
        )
            return Result.Failure<GetAllPostsQuery.Response.PostResponse>(DomainErrors.Post
                .CannotCreatePostWithoutContentOrFiles());


        fileService.Delete(request.DeletedFileUrls);

        var fileUrls = await fileService.UploadAsync(request.Files);
        
        post.Modify(request.Content,fileUrls, request.DeletedFileUrls);
        
        repository.Update(post);

        await repository.SaveChangeAsync(cancellationToken);
        
        return await repository.QueryAsync(post.Id,
            GetAllPostsQuery.Response.Mapper(post.UserId), cancellationToken);

    }
}