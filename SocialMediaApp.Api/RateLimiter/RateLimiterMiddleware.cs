using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Application.Core.Abstraction.Http;

namespace SocialMediaApp.Api.RateLimiter;

public class SecurityApisLimiterPolicy(IHttpService httpService) : IRateLimiterPolicy<string>
{
    public const string PolicyName = nameof(SecurityApisLimiterPolicy);

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: string.Concat(httpContext.Request.Path.ToString().ToLower(),
                httpService.GetCurrentUserId().ToString() ?? httpContext.Connection.RemoteIpAddress?.ToString()),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                AutoReplenishment = true,
                Window = TimeSpan.FromMinutes(1),
            });
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
    };
}