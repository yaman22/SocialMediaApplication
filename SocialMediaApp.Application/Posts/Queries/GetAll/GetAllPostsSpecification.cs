using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Domain.Core.Enums;
using SocialMediaApp.Domain.Core.Specification.SpecificationMapping;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Queries.GetAll;

public class GetAllPostsSpecification : MappingSpecification<Post, GetAllPostsQuery.Response.PostResponse>
{
    public GetAllPostsSpecification(GetAllPostsQuery.Request request, Guid currentUserId)
    {
        AddFilter(post=>!post.DateDeleted.HasValue);
        
        if (!string.IsNullOrEmpty(request.Search)) 
            AddFilter(post=>EF.Functions.Like(post.Content.ToUpper(),$"%{request.Search.ToUpper()}%"));

        if (request.UserId.HasValue)
        {
            AddFilter(post=>post.UserId == request.UserId);    
        }
        
        if (request.OrderType == OrderType.Ascending)
        {
            AddOrderBy(post=>post.DateCreated);
        }
        else
        {
            AddOrderByDescending(post => post.DateCreated);
        }

        
        
        ApplyMapping(GetAllPostsQuery.Response.Mapper(currentUserId));
        AddPagination(request.PageIndex, request.PageSize);
    }
}