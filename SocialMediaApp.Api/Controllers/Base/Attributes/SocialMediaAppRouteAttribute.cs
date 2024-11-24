using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Api.Controllers.Base.Swagger;

namespace SocialMediaApp.Api.Controllers.Base.Attributes;

/// <summary>
/// Generate Apis on specific pattern
/// </summary>
public class SocialMediaAppRouteAttribute : RouteAttribute
{
    /// <summary>
    /// Initialize the fullRoute attribute
    /// </summary>
    /// <param name="routeType">type of the route</param>
    /// <param name="version"></param>
    public SocialMediaAppRouteAttribute(RouteType routeType, int version = 1) : base($"api/v{version}/{routeType}/[controller]/[action]")
    {
    }
}