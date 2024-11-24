namespace SocialMediaApp.Domain.Core.Extensions;

public static class ExtensionMethods
{
	public static DateTimeOffset ToDateTimeOffset(this DateOnly dateOnly, TimeSpan offset) => new DateTimeOffset(dateOnly.ToDateTime(), offset);
	public static DateTime ToDateTime(this DateOnly dateOnly) => new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
	public static DateOnly ToDateOnly(this DateTime dateTime) => new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
	public static Guid? ToGuid(this string? stringId) => string.IsNullOrEmpty(stringId) ? null : new Guid(stringId);

	public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (var element in enumerable) action(element);
	}

	public static async Task ForeachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> func)
	{
		foreach (var element in enumerable) await func(element);
	}

	public static async Task<List<TResult>> SelectAsync<TSource, TResult>(
		this IEnumerable<TSource> enumerable,
		Func<TSource, Task<TResult>> func)
	{
		var result = new List<TResult>();

		foreach (var item in enumerable)
			result.Add(await func(item));

		return result;
	}
}