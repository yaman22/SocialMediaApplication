using System.Net;
using Microsoft.AspNetCore.Identity;
using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Results.Extensions;

/// <summary>
/// rail way oriented design
/// </summary>
public static class ResultExtensions
{
    public static Result<TValue> ToResult<TValue>(this IdentityResult identityResult)
    {
        var errors = identityResult.Errors.Select(e => new
        {
            error = $" {e.Code} : {e.Description}"
        }).ToList();

        
        var errorMessage = string.Join(',', errors);

        return Results.Result.Failure<TValue>(new Error(errorMessage, HttpStatusCode.BadRequest));
    }


    /// <summary>
    /// Create the result object With Nullable Value 
    /// </summary>
    /// <param name="valueTask">The Value With Async Call</param>
    /// <typeparam name="TValue">Any Class</typeparam>
    /// <returns>Value As result object</returns>
    /// <remarks>Most use on FirstOrDefaultAsync() Method</remarks>
    public static async Task<Result<TValue>> AsNullableAsyncResult<TValue>(this Task<TValue?> valueTask)
        => Result<TValue>.Create(await valueTask);

    /// <summary>
    /// Create the result object With Value 
    /// </summary>
    /// <param name="value">The Value With</param>
    /// <typeparam name="TValue">Any Class</typeparam>
    /// <returns>Value As result object</returns>
    /// <remarks>Most use on FirstOrDefault() Method</remarks>
    public static Result<TValue> AsNullableResult<TValue>(this TValue? value)
        => Result<TValue>.Create(value);

    /// <summary>
    /// Create the result object With Nullable Value 
    /// </summary>
    /// <param name="valueTask">The Value With</param>
    /// <typeparam name="TValue">Any Class</typeparam>
    /// <returns>Value As result object</returns>
    /// <remarks>Most use on FirstAsync Method</remarks>
    public static async Task<Result<TValue>> AsAsyncResult<TValue>(this Task<TValue> valueTask)
        => Result<TValue>.Create(await valueTask);

    /// <summary>
    /// Create the result object With Nullable Value 
    /// </summary>
    /// <param name="value">The Value With</param>
    /// <typeparam name="TValue">Any Class</typeparam>
    /// <returns>Value As result object</returns>
    /// <remarks>Most use on First() Method</remarks>
    public static Result<TValue> AsResult<TValue>(this TValue value)
        => Result<TValue>.Create(value);

    /// <summary>
    /// Either return the result object if it's failure or check the predicate function
    /// </summary>
    /// <param name="result">The rail way result object</param>
    /// <param name="predicate">function to determine if the make result object failure or save the previous change</param>
    /// <param name="error">return error if the predicate function return false</param>
    /// <typeparam name="TValue">The value of this result object</typeparam>
    /// <returns>result object with either success with value or failure with error</returns>
    public static Result<TValue> Ensure<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Error error)
    {
        if (result.IsFailure) return result;
        return predicate(result.Value) ? Results.Result.Success(result.Value) : Results.Result.Failure<TValue>(error);
    }

    /// <summary>
    /// Either return the result object if it's failure or check the predicate function
    /// </summary>
    /// <param name="resultTask">The rail way result object</param>
    /// <param name="predicate">function to determine if the make result object failure or save the previous change</param>
    /// <param name="error">return error if the predicate function return false</param>
    /// <typeparam name="TValue">The value of this result object</typeparam>
    /// <returns>result object with either success with value or failure with error</returns>
    /// <remarks>The result is a task(Async) the predicate is not</remarks>
    public static async Task<Result<TValue>> EnsureAsync<TValue>(this Task<Result<TValue>> resultTask,
        Func<TValue, bool> predicate, Error error)
    {
        Result<TValue> result = await resultTask;
        if (result.IsFailure) return result;
        return predicate(result.Value) ? Results.Result.Success(result.Value) : Results.Result.Failure<TValue>(error);
    }

    /// <summary>
    /// Either return the result object if it's failure or check the predicate function
    /// </summary>
    /// <param name="result">The rail way result object</param>
    /// <param name="predicate">function to determine if the make result object failure or save the previous change</param>
    /// <param name="error">return error if the predicate function return false</param>
    /// <typeparam name="TValue">The value of this result object</typeparam>
    /// <returns>result object with either success with value or failure with error</returns>
    /// <remarks>The predicate return a task value(Async)</remarks> 
    public static async Task<Result<TValue>> EnsureAsync<TValue>(this Result<TValue> result,
        Func<TValue, Task<bool>> predicate, Error error)
    {
        if (result.IsFailure) return result;
        return await predicate(result.Value)
            ? Results.Result.Success(result.Value)
            : Results.Result.Failure<TValue>(error);
    }

    /// <summary>
    /// Either return the result object if it's failure or check the predicate function
    /// </summary>
    /// <param name="resultTask">The rail way result object</param>
    /// <param name="predicate">function to determine if the make result object failure or save the previous change</param>
    /// <param name="error">return error if the predicate function return false</param>
    /// <typeparam name="TValue">The value of this result object</typeparam>
    /// <returns>result object with either success with value or failure with error</returns>
    /// <remarks>both the result and predicate are task value</remarks> 
    public static async Task<Result<TValue>> EnsureAsync<TValue>(this Task<Result<TValue>> resultTask,
        Func<TValue, Task<bool>> predicate, Error error)
    {
        Result<TValue> result = await resultTask;
        if (result.IsFailure) return result;
        return await predicate(result.Value)
            ? Results.Result.Success(result.Value)
            : Results.Result.Failure<TValue>(error);
    }
}