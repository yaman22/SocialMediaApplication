using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Persistence.Configurations;

public class CommentConfigurations : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(comment=>comment.User).WithMany(user=>user.Comments).HasForeignKey(comment=>comment.UserId);
        builder.HasOne(comment=>comment.Post).WithMany(post=>post.Comments).HasForeignKey(comment=>comment.PostId);
        builder.HasOne(comment=> comment.BaseComment).WithMany(comment=>comment.Replies).HasForeignKey(comment=>comment.BaseCommentId).OnDelete(DeleteBehavior.Cascade);
    }
}