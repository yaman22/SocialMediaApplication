using System.Security.Claims;
using SocialMediaApp.Domain.Entities.Users;

namespace SocialMediaApp.Application.Core.Abstraction.Security;

public interface IJwtBearerGenerator
{
    public Task<string> GenerateToken(User user);
    public Task<string> GenerateToken(User user, params Claim[] extraClaim);
}