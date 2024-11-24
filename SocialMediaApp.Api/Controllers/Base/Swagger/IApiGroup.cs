namespace SocialMediaApp.Api.Controllers.Base.Swagger;

public interface IApiGroup <TApiGroupNames>
{
    public TApiGroupNames[] GroupNames { get; set; }
}