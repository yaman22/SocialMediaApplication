using SocialMediaApp.Domain.Core.Specification.SpecificationMapping;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Queries.GetAllLikes;

public class GetAllLikesSpecification : MappingSpecification<Like, GetAllLikesQuery.Response.LikeResponse>
{
    public GetAllLikesSpecification(GetAllLikesQuery.Request request)
    {
        AddFilter(like=>!like.DateDeleted.HasValue && like.PostId == request.PostId && !like.Post.DateDeleted.HasValue);
        
        AddOrderByDescending(like => like.DateCreated);
        ApplyMapping(GetAllLikesQuery.Response.Mapper());
        AddPagination(request.PageIndex,request.PageSize);
    }
}