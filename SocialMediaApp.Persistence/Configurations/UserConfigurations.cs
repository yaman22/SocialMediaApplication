using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.UseTphMappingStrategy();
        builder.HasMany(user=>user.Comments).WithOne(comment=>comment.User).HasForeignKey(comment=>comment.UserId);
        builder.HasMany(user=>user.Likes).WithOne(like=>like.User).HasForeignKey(like=>like.UserId);
        builder.HasMany(user=>user.Posts).WithOne(post=>post.User).HasForeignKey(post=>post.UserId);
    }
}