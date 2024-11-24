using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Domain.Entities.Users;
using SocialMediaApp.Domain.Enums;
using SocialMediaApp.Domain.Shared.DTOs;

namespace SocialMediaApp.Application.Users.Commands.SignUp;

public class SignUpUserCommand
{
    public class Request : IRequest<Response>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string? UserName { get; init; }
        public string PhoneNumber { get; init; }
        public string Email { get; init; }
        
        [MinLength(8), MaxLength(64)]
        public string Password { get; init; }
        
        public Gender Gender { get; init; }
        public DateTime BirthDate { get; init; }
        public IFormFile? Image { get; init; }
        public string? Bio { get; init; }
    }
    
    public class Response : TokenDto
    {
        public Guid Id { get; init; }
        public string UserName { get; init; }
        public string? ImageUrl { get; init; }
        
        public static Expression<Func<User, Response>> Mapper(string accessToken) => user
            => new()
            {
                Id = user.Id,
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = user.PasswordHash!
            };
    }
}