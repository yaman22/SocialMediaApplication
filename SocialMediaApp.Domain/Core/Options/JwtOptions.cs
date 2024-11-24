namespace SocialMediaApp.Domain.Core.Options;

public class JwtOptions
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public TimeSpan Expire { get; set; } 
}