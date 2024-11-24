using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Results;

public class Result
{
    protected Result()
    {
    }

    protected Result(bool isSuccess, Error error) => (IsSuccess, Error) = (isSuccess, error);

    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; private set; }

    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(error);
}