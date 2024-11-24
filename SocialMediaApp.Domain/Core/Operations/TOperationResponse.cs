using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Domain.Core.Operations;

public class OperationResponse<TResponse> : OperationResponse where TResponse : class
{
    public static OperationResponse<TResponse> Create(TResponse? response) => new(response, true, Error.None);
    private OperationResponse(TResponse? response, bool isSuccess, Error error) : base(isSuccess, error) => Response = response;

    public TResponse? Response { get; set; }
    public TResponse Value => IsSuccess ? Response! : throw new ArgumentNullException();


    public static implicit operator OperationResponse<TResponse>(Result<TResponse> result) => result.IsSuccess ? result.Value : result.Error;
    public static OperationResponse<TResponse> SetSuccess<TResponse>(TResponse response) where TResponse : class => new(response, true, Error.None);
    public static OperationResponse<TResponse> SetFailed<TResponse>(Error error) where TResponse : class => new(null, false, error);
    public static implicit operator OperationResponse<TResponse>(Error error) => new(null, false, error);
    public static implicit operator OperationResponse<TResponse>(TResponse value) => new(value, true, Error.None);
}