using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Api.Middlewares.Seed;

public static class SeedMiddleWare
{
    public static async Task<IApplicationBuilder> MigrationAsync<TContext>(this IApplicationBuilder builder,
        Func<TContext, IServiceProvider, Task> seed, bool deleteDbIfFailure = false) where TContext : DbContext
    {
        var serviceProvider = builder.ServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceProvider.GetRequiredService<TContext>();
        try
        {
            logger.LogInformation("Migrating....");
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrate is done");

            logger.LogInformation("Seeding....");
            await seed(context, serviceProvider);
            logger.LogInformation("Seed is done");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed while applies migrations/seed data");

            if (deleteDbIfFailure)
            {
                await context.Database.EnsureCreatedAsync();
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
                await seed(context, serviceProvider);
            }
        }

        return builder;
    }

    
    
    
    private static IServiceProvider ServiceProvider(this IApplicationBuilder builder) => builder
        .ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope()
        .ServiceProvider;
}