using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Application.Core.CQRS;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : class?
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task<Result> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}