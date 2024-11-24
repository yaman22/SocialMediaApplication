using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Persistence.Configurations;

public class PostConfigurations : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasOne(post => post.User).WithMany(user => user.Posts).HasForeignKey(post => post.UserId);
        builder.HasMany(post=>post.Comments).WithOne(comment => comment.Post).HasForeignKey(comment => comment.PostId);
        builder.HasMany(post=>post.Likes).WithOne(like => like.Post).HasForeignKey(like => like.PostId);
        
    }
}