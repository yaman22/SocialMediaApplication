using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using SocialMediaApp.Domain.Core.Errors;
using SocialMediaApp.Domain.Core.Exceptions.Base;

namespace SocialMediaApp.Api.Middlewares.GlobalExceptionHandler;

/// <inheritdoc />
public class GlobalExceptionHandler : IExceptionHandler
{
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	/// <inheritdoc />
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var error = exception switch
		{
			DomainException domainException => domainException,
			not null => Error.Create(exception),
			_ => throw new ArgumentOutOfRangeException()
		};
		
		var responseToWrite = new
		{
			error.Message,
			StatusCode = (int)error.StatusCode,
		};

		httpContext.Response.StatusCode = (int)error.StatusCode;
		httpContext.Response.ContentType = "application/json";
		await httpContext.Response.WriteAsync(JsonSerializer.Serialize(responseToWrite), cancellationToken: cancellationToken);
		return true;
	}
}