using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Entities.Posts;
using SocialMediaApp.Domain.Entities.Users;
using SocialMediaApp.Domain.Enums;
using SocialMediaApp.Persistence.Context;

namespace SocialMediaApp.Persistence.Seeds;

public class DataSeeder
{
    public static async Task Seed(ApplicationDbContext context,IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetService<UserManager<User>>()!;

        SeedWwwroot();
        await SeedUsers(context, userManager);
        await SeedPosts(context, serviceProvider, userManager);
        await SeedComments(context, serviceProvider, userManager);
        await SeedLikes(context, serviceProvider, userManager);
    }


    private static async Task SeedUsers(ApplicationDbContext context, UserManager<User> userManager)
    {
        if (context.Users.Count() >= 4)
            return;

        var faker = new Faker();
        Random random = new Random();
        if (!await context.Users.AnyAsync(user => user.Email == "user@gmail.com"))
        {
            var user0 = new User(
                faker.Name.FirstName(Name.Gender.Female),
                faker.Name.LastName(Name.Gender.Female),
                faker.Person.UserName + random.Next(1, 10),
                AddImage(ConstValues.femaleUser),
                Gender.Female,
                faker.Person.DateOfBirth,
                "user0@gmail.com",
                faker.Person.Phone,
                faker.Lorem.Text()
            );
            await userManager.CreateAsync(user0, "12345678");
        }

        var user1 = new User(
            faker.Name.FirstName(Name.Gender.Female),
            faker.Name.LastName(Name.Gender.Female),
            faker.Person.UserName + random.Next(10, 20),
            AddImage(ConstValues.femaleUser),
            Gender.Female,
            faker.Person.DateOfBirth,
            "user1@gmail.com",
            faker.Person.Phone,
            faker.Lorem.Text()
        );
        await userManager.CreateAsync(user1, "12345678");

        var user2 = new User(
            faker.Name.FirstName(Name.Gender.Female),
            faker.Name.LastName(Name.Gender.Female),
            faker.Person.UserName + random.Next(20, 30),
            AddImage(ConstValues.femaleUser),
            Gender.Female,
            faker.Person.DateOfBirth,
            "user2@gmail.com",
            faker.Person.Phone,
            faker.Lorem.Text()
        );
        await userManager.CreateAsync(user2, "12345678");

        var user3 = new User(
            faker.Name.FirstName(Name.Gender.Male),
            faker.Name.LastName(Name.Gender.Male),
            faker.Person.UserName + random.Next(30, 40),
            AddImage(ConstValues.maleUser),
            Gender.Female,
            faker.Person.DateOfBirth,
            "user3@gmail.com",
            faker.Person.Phone,
            faker.Lorem.Text()
        );
        await userManager.CreateAsync(user3, "12345678");

        var user4 = new User(
            faker.Name.FirstName(Name.Gender.Male),
            faker.Name.LastName(Name.Gender.Male),
            faker.Person.UserName + random.Next(40, 50),
            AddImage(ConstValues.maleUser),
            Gender.Female,
            faker.Person.DateOfBirth,
            "user4@gmail.com",
            faker.Person.Phone,
            faker.Lorem.Text()
        );
        await userManager.CreateAsync(user4, "12345678");

        // await context.SaveChangesAsync();
    }

    private static async Task SeedPosts(ApplicationDbContext context, IServiceProvider serviceProvider,
        UserManager<User> userManager)
    {
        if (context.Posts.Count() >= 4)
            return;

        var users = await context.Users.Select(user => user.Id).ToListAsync();

        if (users.Count <= 4)
        {
            await SeedUsers(context, userManager);
            users = await context.Users.Select(user => user.Id).ToListAsync();
        }

        if (users.FirstOrDefault() != null)
        {
            var post1User1 = new Post(users.First(), "Nissan GTR is a monster.",
                [AddImage(ConstValues.GTR)]);
            await context.AddAsync(post1User1);


            var post2User2 = new Post(users.Skip(1).First(),
                "This is the king of the Sky, but after the Russian SU-57 Felon",
                [AddImage(ConstValues.F22Rapture)]);
            await context.AddAsync(post2User2);


            var post3User3 = new Post(users.Skip(2).First(), "just relax", [ConstValues.laptop]);
            await context.AddAsync(post3User3);


            var post4User4 = new Post(users.Skip(2).First(), "time for some fun", [ConstValues.coloredLaptop]);
            await context.AddAsync(post4User4);

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedComments(ApplicationDbContext context, IServiceProvider serviceProvider,
        UserManager<User> userManager)
    {
        var users = await context.Users.Select(user => user.Id).ToListAsync();
        if (users.Count <= 4)
        {
            await SeedUsers(context, userManager);
            users = await context.Users.Select(user => user.Id).ToListAsync();
        }

        var posts = await context.Posts.Select(p => p.Id).ToListAsync();
        if (posts.Count <= 4)
        {
            await SeedPosts(context, serviceProvider, userManager);
            posts = await context.Posts.Select(p => p.Id).ToListAsync();
        }

        if (users.Count < 4 || posts.Count < 4)
            return;

        var comment1Post1 = new Comment(users.First(), posts.First(), "Wow that is fantastic", null, null);
        await context.AddAsync(comment1Post1);

        var comment2Post2 = new Comment(users.Skip(1).First(), posts.Skip(1).First(), "I agree a lot", null, null);
        await context.AddAsync(comment2Post2);

        var comment3Post3 = new Comment(users.Skip(2).First(), posts.Skip(2).First(), "I think so", null, null);
        await context.AddAsync(comment3Post3);

        var comment4Post4 = new Comment(users.Skip(3).First(), posts.Skip(3).First(), "good post", null, null);
        await context.AddAsync(comment4Post4);

        await context.SaveChangesAsync();
    }

    private static async Task SeedLikes(ApplicationDbContext context, IServiceProvider serviceProvider,
        UserManager<User> userManager)
    {
        var users = await context.Users.Select(user => user.Id).ToListAsync();
        if (users.Count <= 4)
        {
            await SeedUsers(context, userManager);
            users = await context.Users.Select(user => user.Id).ToListAsync();
        }

        var posts = await context.Posts.Select(p => p.Id).ToListAsync();
        if (posts.Count < 4)
        {
            await SeedPosts(context, serviceProvider, userManager);
            posts = await context.Posts.Select(p => p.Id).ToListAsync();
        }

        if (users.Count <= 4 || posts.Count < 4)
            return;

        var like1Post1 = new Like(users.First(), posts.First());
        await context.AddAsync(like1Post1);

        var like2Post2 = new Like(users.Skip(1).First(), posts.Skip(1).First());
        await context.AddAsync(like2Post2);

        var like3Post3 = new Like(users.Skip(2).First(), posts.Skip(2).First());
        await context.AddAsync(like3Post3);

        var like4Post4 = new Like(users.Skip(3).First(), posts.Skip(3).First());
        await context.AddAsync(like4Post4);

        await context.SaveChangesAsync();
    }

    private static void SeedWwwroot()
    {
        if (!Directory.Exists(ConstValues.wwwroot))
        {
            System.IO.Compression.ZipFile.ExtractToDirectory("wwwroot.zip", Directory.GetCurrentDirectory());
        }
    }

    public static string AddImage(string imageName)
    {
        var source = Path.Combine(Directory.GetCurrentDirectory(), ConstValues.wwwroot, imageName);
        var path = Path.Combine(ConstValues.Seed, Guid.NewGuid() + "_" + imageName);
        var dest = Path.Combine(Directory.GetCurrentDirectory(), ConstValues.wwwroot, path);
        File.Copy(source, dest);
        return path;
    }
}