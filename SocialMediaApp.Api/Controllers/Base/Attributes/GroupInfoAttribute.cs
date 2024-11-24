namespace SocialMediaApp.Api.Controllers.Base.Attributes;

public sealed class GroupInfoAttribute : Attribute
{
    public GroupInfoAttribute(string title, string description, string version)
    {
        Title = title;
        Version = version;
        Description = description;
    }

    public string Title { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
}