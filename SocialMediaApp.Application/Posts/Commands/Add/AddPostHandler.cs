using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAll;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.Add;

public class AddPostHandler(IRepository repository, IHttpService httpService, IFileService fileService)
    : IRequestHandler<AddPostCommand.Request, GetAllPostsQuery.Response.PostResponse>
{
    public async Task<Result<GetAllPostsQuery.Response.PostResponse>> HandleAsync(AddPostCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Content) && (request.Files is null || !request.Files.Any()))
            return Result.Failure<GetAllPostsQuery.Response.PostResponse>(DomainErrors.Post
                .CannotCreatePostWithoutContentOrFiles());

        var fileUrls = await fileService.UploadAsync(request.Files);

        var post = new Post(httpService.GetCurrentUserId().Value, request.Content, fileUrls);

        repository.Add(post);

        await repository.SaveChangeAsync(cancellationToken);

        return await repository.QueryAsync(post.Id,
            GetAllPostsQuery.Response.Mapper(httpService.GetCurrentUserId().Value), cancellationToken);
    }
}