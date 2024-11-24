using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scrutor;
using SocialMediaApp.Application.Core.Abstraction.Data;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Entities.Users;
using SocialMediaApp.Persistence.Context;
using SocialMediaApp.Persistence.Repositories;

namespace SocialMediaApp.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configurations,
        IWebHostEnvironment webHostEnvironment)
    {
        services.Scan(CommandQueryHandlers);
        services.AddScoped<IRepository, Repository>(); 
        services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(DataBaseOption);
        return services;
        
        
        #region HelperMethods

        //database
        void DataBaseOption(DbContextOptionsBuilder options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            
            options.UseNpgsql(configurations.GetConnectionString(ConstValues.DefaultConnection), optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            if (webHostEnvironment.IsDevelopment()) options.EnableSensitiveDataLogging();
        }
        
        //Handlers
        void CommandQueryHandlers(ITypeSourceSelector selector)
        {
            selector
                .FromAssemblies(Application.AssemblyReference.Assembly)
                .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            selector
                .FromAssemblies(Application.AssemblyReference.Assembly)
                .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();;
        }


        #endregion
    }


    
}