using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Operations;

public static class OperationResponseExtensions
{
    public static OperationResponse<T> EnsureNotNull<T>(this OperationResponse<T> operationResponse, Error error) where T : class
    {
        if (operationResponse.IsFailure) return operationResponse;
        return operationResponse.Response is null ? error : operationResponse;
    }

    public static OperationResponse<T> EnsureNotNull<T>(this OperationResponse<T> operationResponse) where T : class
    {
        if (operationResponse.IsFailure) return operationResponse;
        return operationResponse.Response is null ? Error.NullValue : operationResponse.Response!;
    }

    public static OperationResponse<T> If<T>(this OperationResponse<T> operationResponse, Func<T, bool> predicate, Error error) where T : class
    {
        if (operationResponse.IsFailure) return operationResponse;
        return !predicate(operationResponse.Response!) ? operationResponse.Response! : error;
    }

    public static async Task<OperationResponse<T>> If<T>(this OperationResponse<T> operationResponse, Func<T, Task<bool>> predicate, Error error) where T : class
    {
        if (operationResponse.IsFailure) return operationResponse;
        return !await predicate(operationResponse.Response!) ? operationResponse.Response! : error;
    }

    public static async Task<OperationResponse<T>> Ensure<T>(this OperationResponse<T> operationResponse, Func<T, Task<bool>> predicate, Error error) where T : class
    {
        if (operationResponse.IsFailure) return operationResponse;
        return await predicate(operationResponse.Response!) ? operationResponse.Response! : error;
    }

    public static OperationResponse<TOut> Map<TIn, TOut>(this OperationResponse<TIn> operationResponse, Func<TIn, TOut> mappingFunc) where TOut : class where TIn : class
        => operationResponse.IsSuccess ? mappingFunc(operationResponse.Response!) : operationResponse.Error;

    public static async Task<OperationResponse<TIn>> Tap<TIn>(this OperationResponse<TIn> operationResponse, Func<Task> func) where TIn : class
    {
        if (operationResponse.IsSuccess) await func();
        return operationResponse;
    }

    public static async Task<OperationResponse<TIn>> Tap<TIn>(this Task<OperationResponse<TIn>> operationResponseTask, Func<Task> func) where TIn : class
    {
        var operationResponse = await operationResponseTask;
        if (operationResponse.IsSuccess) await func();
        return operationResponse;
    }

    public static async Task<OperationResponse<TOut>> Map<TIn, TOut>(this Task<OperationResponse<TIn>> operationResponseTask, Func<TIn, TOut> funcMapping) where TIn : class where TOut : class
    {
        OperationResponse<TIn> operationResponse = await operationResponseTask;
        return operationResponse.IsFailure ? operationResponse.Error : funcMapping(operationResponse.Response!);
    }
}