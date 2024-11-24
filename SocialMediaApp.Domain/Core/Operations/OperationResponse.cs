using System.Text.Json;
using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Operations;

public class OperationResponse
{
    protected OperationResponse(bool isSuccess, Error error) => (IsSuccess, Error) = (isSuccess, error);

    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; private set; }

    public static OperationResponse Success() => new(true, Error.None);
    public static OperationResponse Failure(Error error) => new OperationResponse(false, error);
    public static implicit operator OperationResponse(Error error) => new OperationResponse(true, error);

    public override string ToString() => IsFailure ? JsonSerializer.Serialize(Error) : JsonSerializer.Serialize(IsSuccess);
}