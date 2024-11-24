using System.ComponentModel;
using System.Linq.Expressions;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Entities.Users;
using SocialMediaApp.Domain.Shared.DTOs;

namespace SocialMediaApp.Application.Users.Commands.LogIn;

public class LogInUserCommand 
{
    public class Request : IRequest<Response>
    {
        [DefaultValue("user0@gmail.com")] public string Email { get; init; }

        [DefaultValue("12345678")] public string Password { get; init; }
    }
    
    public class Response : TokenDto
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string? UserName { get; init; }
        public string? ImageUrl { get; init; }

        public static Expression<Func<User, Response>> Mapper(string accessToken) => user
            => new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                ImageUrl = user.ImageUrl,
                RefreshToken = user.PasswordHash,
                AccessToken = accessToken,
            };
    }
}