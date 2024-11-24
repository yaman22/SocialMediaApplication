using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.Domain.Entities.Posts;

namespace SocialMediaApp.Persistence.Configurations;

public class LikeConfigurations : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasOne(like=>like.User).WithMany(user=>user.Likes).HasForeignKey(like=>like.UserId);
        builder.HasOne(like=>like.Post).WithMany(post=>post.Likes).HasForeignKey(like=>like.PostId);
    }
}