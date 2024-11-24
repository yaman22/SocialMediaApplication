using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Results;
using SocialMediaApp.Domain.Core.ValidationResult;

namespace SocialMediaApp.Api.Controllers.Base;

/// <summary>
/// Base Api Controller For All Controllers
/// </summary>
[ApiController]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Handle Validation Errors
    /// </summary>
    /// <param name="result">result object</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>IActionResult Response</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected IActionResult HandleFailure(Result result) => result switch
    {
        { IsSuccess: true } => throw new InvalidOperationException(),
        IValidationResult validationResult => BadRequest(
            CreateProblemDetails(
                "Validation Error", 
                StatusCodes.Status400BadRequest,
                result.Error, 
                validationResult.Errors)),
        _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
    };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) => new()
        {
            Title = title,
            Type = error.StatusCode.ToString(),
            Detail = error.Message,
            Extensions = { { nameof(errors), errors } },
            Status = status
        };
}