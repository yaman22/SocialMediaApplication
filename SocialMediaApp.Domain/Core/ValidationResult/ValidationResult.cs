using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Domain.Core.ValidationResult;

public class ValidationResult : Result, IValidationResult
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        => Errors = errors;

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}