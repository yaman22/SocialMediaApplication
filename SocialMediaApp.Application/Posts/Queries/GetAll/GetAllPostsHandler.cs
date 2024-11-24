using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Application.Posts.Queries.GetAll;

public class GetAllPostsHandler(IRepository repository, IHttpService httpService) : IRequestHandler<GetAllPostsQuery.Request, GetAllPostsQuery.Response>
{
    public async Task<Result<GetAllPostsQuery.Response>> HandleAsync(GetAllPostsQuery.Request request, CancellationToken cancellationToken = default)
    {
        var specification = new GetAllPostsSpecification(request,httpService.GetCurrentUserId().Value);
        
        
        return Result.Success(new GetAllPostsQuery.Response()
        {
            Posts = await repository.QueryAsync(specification, cancellationToken),
            Count = await repository.Query(specification.Criteria.ToArray()).CountAsync(cancellationToken),
        });
    }
}