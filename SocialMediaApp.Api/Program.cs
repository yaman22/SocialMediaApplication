using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SocialMediaApp.Api;
using SocialMediaApp.Api.Controllers.Base.Swagger;
using SocialMediaApp.Infrastructure;
using SocialMediaApp.Persistence;
using SocialMediaApp.Persistence.Context;
using SocialMediaApp.Persistence.Seeds;
using SocialMediaApp.Api.Middlewares.Seed;
using SocialMediaApp.Api.RateLimiter;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Configuration.AddJsonFiles(builder.Environment);
builder.WebHost.UseKestrel();
builder.Services.Configure<IdentityOptions>(ConfigurationMethods.IdentityOptions);
builder.Services.AddControllers().AddJsonOptions(ConfigurationMethods.JsonOptions);
builder.Services.AddLogging(o => o.AddConfiguration(builder.Configuration));
builder.Services.AddCors(options => options.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, SecurityApisLimiterPolicy>(SecurityApisLimiterPolicy.PolicyName);
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ",
            cancellationToken: token);
    };
});

builder.Services.AddMemoryCache(option =>
{
    option.SizeLimit = 1024 * 1024;
});

var services = builder.Services.AddSwaggerGen(builder.SwaggerOptions);

(services, builder.Environment).AddApiGroupNames<RouteType>(true);

builder.Services.AddValidatorsFromAssemblies([
    SocialMediaApp.Application.AssemblyReference.Assembly
]);

builder.Services.AddPersistence(builder.Configuration, builder.Environment).AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    app.UseSwagger<RouteType>();
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders();
app.UseStaticFiles();
app.UseCors();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSwagger();
});

#pragma warning disable ASP0000
await app.MigrationAsync<ApplicationDbContext>(DataSeeder.Seed, app.Environment.IsDevelopment());
#pragma warning restore ASP0000
app.Run();

app.Run();
