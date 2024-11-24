namespace SocialMediaApp.Api.Controllers.Base.Swagger;

public class ApiGroupAttribute : Attribute, IApiGroup<RouteType>
{
    public ApiGroupAttribute(params RouteType[] names)
    {
        GroupNames = names;
    }
    
    public RouteType[] GroupNames { get; set; }
}