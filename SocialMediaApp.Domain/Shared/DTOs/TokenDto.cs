namespace SocialMediaApp.Domain.Shared.DTOs;

public class TokenDto
{
    public TokenDto()
    {
    }

    public TokenDto(string refreshToken, string accessToken) => (RefreshToken, AccessToken) = (refreshToken, accessToken);
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}