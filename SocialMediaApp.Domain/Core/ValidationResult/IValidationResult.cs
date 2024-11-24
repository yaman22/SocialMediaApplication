using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.ValidationResult;

public interface IValidationResult
{
    Error[] Errors { get; }
}