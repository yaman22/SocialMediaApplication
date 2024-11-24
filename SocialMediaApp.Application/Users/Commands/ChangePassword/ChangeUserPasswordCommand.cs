using System.ComponentModel.DataAnnotations;
using SocialMediaApp.Application.Core.CQRS;

namespace SocialMediaApp.Application.Users.Commands.ChangePassword;

public class ChangeUserPasswordCommand
{
    public class Request : IRequest
    {
        public string OldPassword { get; init; }
        
        [MinLength(8), MaxLength(64)]
        public string NewPassword { get; init; }
    }
}