using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Application.Core.Abstraction.Security;
using SocialMediaApp.Domain.Core.Options;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Infrastructure.Security;

public class JwtBearerGenerator : IJwtBearerGenerator
{
    private readonly JwtOptions _jwtOptions;
    private readonly UserManager<User> _userManager;

    public JwtBearerGenerator(IOptions<JwtOptions> jwtOptions, UserManager<User> userManager)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> GenerateToken(User user) => await GenerateToken(user, []);

    public async Task<string> GenerateToken(User user, params Claim[] extraClaim)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaimsAsync(user);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.Now.Add(_jwtOptions.Expire),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task<IEnumerable<Claim>> GetClaimsAsync(User user, params Claim[] extraClaims)
    {
        return new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), }
            .Union((await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)))
            .Union(await _userManager.GetClaimsAsync(user))
            .Union(extraClaims);
    }
}