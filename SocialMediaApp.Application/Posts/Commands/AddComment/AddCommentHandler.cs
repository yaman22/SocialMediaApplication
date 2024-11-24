using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Posts.Queries.GetAllComments;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Commands.AddComment;

public class AddCommentHandler(IRepository repository, IHttpService httpService, IFileService fileService)
    : IRequestHandler<AddCommentCommand.Request,GetAllCommentsQuery.Response.CommentResponse>
{
    public async Task<Result<GetAllCommentsQuery.Response.CommentResponse>> HandleAsync(AddCommentCommand.Request request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Content) && request.File is null)
            return Result.Failure<GetAllCommentsQuery.Response.CommentResponse>(DomainErrors.Comment.CommentMustHaveTextOrFile());

        var fileUrl = await fileService.UploadAsync(request.File);

        var comment = new Comment(httpService.GetCurrentUserId().Value, request.PostId, request.Content,
            fileUrl, request.BaseCommentId);

        repository.Add(comment);

        await repository.SaveChangeAsync(cancellationToken);

        return await repository.QueryAsync(comment.Id, GetAllCommentsQuery.Response.Mapper(),cancellationToken);
    }
}