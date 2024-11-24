using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Domain.Core.Results;

namespace SocialMediaApp.Api.Controllers.Base.Extensions;

/// <summary>
/// Basic extension methods for controller 
/// </summary>
public static class ControllerExtensions
{
	/// <summary>
	/// Convert a result type to jsonResult 
	/// </summary>
	/// <param name="resultTask"></param>
	/// <typeparam name="TResponse"></typeparam>
	/// <returns></returns>
	public static async Task<JsonResult> ToJsonResultAsync<TResponse>(this Task<Result<TResponse>> resultTask) where TResponse : class?
	{
		var result = await resultTask;

		return result.IsSuccess switch
		{
			true => new JsonResult(new
			{
				Message = "Success",
				StatusCode = 200,
				Response = result.Value,
			})
			{
				StatusCode = 200,
			},

			false => new JsonResult(new
			{
				Message = result.Error.Message,
				StatusCode = (int)result.Error.StatusCode,
				// Resposne = new object()
			})
			{
				ContentType = "application/json",
				StatusCode = (int)result.Error.StatusCode,
			}
		};
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="resultTask"></param>
	/// <returns></returns>
	public static async Task<JsonResult> ToJsonResultAsync(this Task<Result> resultTask)
	{
		var result = await resultTask;
		return result.IsSuccess switch
		{
			true => new JsonResult(new
			{
				Message = "Success",
				StatusCode = 200,
				IsSuccess = true
			})
			{ StatusCode = 200 },
			false => new JsonResult(new
			{
				Message = result.Error.Message,
				StatusCode = (int)result.Error.StatusCode,
				IsSuccess = false,
			})
			{
				ContentType = "application/json",
				StatusCode = (int)result.Error.StatusCode,
			}
		};
	}
}