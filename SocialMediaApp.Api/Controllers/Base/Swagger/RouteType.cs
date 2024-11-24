using SocialMediaApp.Api.Controllers.Base.Attributes;

namespace SocialMediaApp.Api.Controllers.Base.Swagger;

/// <summary>
/// Specify the api type for different api pattern
/// </summary>
/// <example>api/dashboard/Users/GetAll</example>
public enum RouteType
{
    /// <summary>
    /// for mobile routs
    /// </summary>
    [GroupInfo(title: "Application", description: "", version: "v1")]
    Application = 1,
    
}