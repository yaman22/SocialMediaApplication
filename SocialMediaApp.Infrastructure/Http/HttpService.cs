using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Domain.Core.Extensions;

namespace SocialMediaApp.Infrastructure.Http;

public class HttpService : HttpContextAccessor, IHttpService
{
    public Guid? GetCurrentUserId()
    {
        try
        {
            Guid? id = Guid.Empty;
             id = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToGuid();

            if (id.HasValue)
            {
                return id;
            }
            
            
            
            var token = HttpContext?.Request.Headers.Authorization.Select(a => a).ToList().FirstOrDefault()
                .Replace("Bearer ", "");
            if (token is null)
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var claims = jwtSecurityToken.Claims;
            var idFromToken = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            id = new Guid(idFromToken);
            return id;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}