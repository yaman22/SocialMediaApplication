using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Queries.GetAllComments;

public class GetAllCommentsHandler(IRepository repository) : IRequestHandler<GetAllCommentsQuery.Request, GetAllCommentsQuery.Response>
{
    public async Task<Result<GetAllCommentsQuery.Response>> HandleAsync(GetAllCommentsQuery.Request request,
        CancellationToken cancellationToken = default)
    {
        var specification = new GetAllCommentsSpecification(request);
        
        return new GetAllCommentsQuery.Response()
        {
            Comments = await repository.QueryAsync(specification, cancellationToken),
            Count = await repository.Query(specification.Criteria.ToArray()).CountAsync(cancellationToken),
        };
    }

}