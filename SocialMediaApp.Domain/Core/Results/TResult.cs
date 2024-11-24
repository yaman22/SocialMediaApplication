using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Results;

public class Result<TValue> : Results.Result
{
    private readonly TValue? _value;

    public static Result<TValue> Create(TValue? value)
        => value is null ? new Result<TValue>(Error.NullValue) : new Result<TValue>(value);

    protected internal Result(TValue value) : base(true, Error.None)
        => _value = value;

    public static Result<TValue> Ensure(TValue value, Func<TValue, bool> predicate, Error error)
        => predicate(value) ? Success(value) : Failure<TValue>(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static Result<TValue> Combine(params Result<TValue>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
        }

        return Success(results[0].Value);
    }

    public static Result<TValue> Ensure(TValue value, params (Func<TValue, bool> predicate, Error error)[] functions)
    {
        foreach ((Func<TValue, bool> predicate, Error error) in functions)
        {
        }

        return Success(value);
    }

    protected internal Result(Error error) : base(false, error)
    {
    }

    public TValue Value => IsSuccess ? _value! : throw new ArgumentNullException($"{nameof(Value)} cannot be null");
}