using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Core.Abstraction.Data;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(dbContextOptions), IApplicationDbContext
{
    public override DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}