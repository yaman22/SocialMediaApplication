using Microsoft.AspNetCore.Http;

namespace SocialMediaApp.Application.Core.Abstraction.Http;

public interface IHttpService : IHttpContextAccessor
{
    Guid? GetCurrentUserId();
}