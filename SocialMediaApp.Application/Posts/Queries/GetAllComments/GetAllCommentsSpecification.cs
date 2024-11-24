using SocialMediaApp.Domain.Core.Specification.SpecificationMapping;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Application.Posts.Queries.GetAllComments;

public class GetAllCommentsSpecification : MappingSpecification<Comment, GetAllCommentsQuery.Response.CommentResponse>
{
    public GetAllCommentsSpecification(GetAllCommentsQuery.Request request)
    {
        AddFilter(comment=> !comment.Post.DateDeleted.HasValue);
        
        if (request.BaseCommentId.HasValue)
            AddFilter(comment => comment.BaseCommentId == request.BaseCommentId.Value && !comment.DateDeleted.HasValue);
        else
            AddFilter(comment => !comment.BaseCommentId.HasValue);

        if (request.PostId.HasValue)
        {
            AddFilter(comment => comment.PostId == request.PostId.Value && !comment.DateDeleted.HasValue);
        }


        AddOrderByDescending(comment => comment.DateCreated);
        ApplyMapping(GetAllCommentsQuery.Response.Mapper());
        AddPagination(request.PageIndex, request.PageSize);
    }
}