using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.CQRS;
using SocialMediaApp.Application.Users.Queries.GetProfile;
using SocialMediaApp.Domain.Enums;

namespace SocialMediaApp.Application.Users.Commands.Modify;

public class ModifyUserCommand
{
    public class Request : IRequest<GetUserProfileQuery.Response>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public Gender Gender { get; init; }
        public IFormFile? Image { get; init; }
        public string? Bio { get; init; }
        public DateTime BirthDate { get; init; }
    }
}