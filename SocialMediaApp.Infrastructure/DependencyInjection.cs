using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Application.Core.Abstraction.Caching;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Application.Core.Abstraction.Security;
using SocialMediaApp.Domain.Core.Options;
using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Infrastructure.Caching;
using SocialMediaApp.Infrastructure.Http;
using SocialMediaApp.Infrastructure.IO;
using SocialMediaApp.Infrastructure.Security;

namespace SocialMediaApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileService,FileService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddSingleton<ICacheService, CacheService>();
        
        services.Configure<JwtOptions>(options => configuration.GetSection(ConstValues.JwtOptions).Bind(options));
        var jwtOptions = configuration.GetSection(ConstValues.JwtOptions).Get<JwtOptions>()!;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["authorization"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },
            };
        }).Services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        }).AddTransient<IJwtBearerGenerator, JwtBearerGenerator>();
        
        return services;
    }
    
    public static void JsonOptions(JsonOptions options)
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
    
   
    
}