using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Application.Posts.Queries.GetAllLikes;

public class GetAllLikesHandler(IRepository repository) : IRequestHandler<GetAllLikesQuery.Request, GetAllLikesQuery.Response>
{
    public async Task<Result<GetAllLikesQuery.Response>> HandleAsync(GetAllLikesQuery.Request request, CancellationToken cancellationToken = default)
    {
        var specification = new GetAllLikesSpecification(request);
        
        return new GetAllLikesQuery.Response()
        {
            Likes = await repository.QueryAsync(specification, cancellationToken),
            Count = await repository.Query(specification.Criteria.ToArray()).CountAsync(cancellationToken),
        };
    }
}