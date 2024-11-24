using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Extensions;

public static class BaseExpressions
{
    /// <summary>
    /// Filter Deleted Record On Queries
    /// </summary>
    /// <typeparam name="TEntity">Entity Represent An Table on Database</typeparam>
    /// <returns>Expression That Represent the filter function</returns>
    public static Expression<Func<TEntity, bool>> FilterDeletedEntity<TEntity>() where TEntity : IBaseEntity
        => entity => !entity.DateDeleted.HasValue;

    /// <summary>
    /// Order Query By DateCreated
    /// </summary>
    /// <typeparam name="TEntity">Entity Represent An Table on Database</typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, DateTimeOffset>> OrderByDateCreated<TEntity>() where TEntity : IBaseEntity
        => entity => entity.DateCreated;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDates<TEntity>(DateTimeOffset startDate, DateTimeOffset endDate) where TEntity : IBaseEntity
        => entity => startDate <= entity.DateCreated && entity.DateCreated <= endDate;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDates<TEntity>(DateTimeOffset? startDate, DateTimeOffset? endDate) where TEntity : IBaseEntity
        => entity => (!startDate.HasValue || startDate <= entity.DateCreated) && (!endDate.HasValue || entity.DateCreated <= endDate);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDatesOrLast6Months<TEntity>(DateTimeOffset? startDate, DateTimeOffset? endDate) where TEntity : IBaseEntity
        => BetweenDates<TEntity>(startDate ?? DateTimeOffset.UtcNow.AddMonths(-6), endDate ?? DateTimeOffset.UtcNow);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="offset"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDates<TEntity>(DateOnly startDate, DateOnly endDate, TimeSpan offset) where TEntity : IBaseEntity
        => entity => startDate.ToDateTimeOffset(offset) <= entity.DateCreated && entity.DateCreated <= endDate.ToDateTimeOffset(offset);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="offset"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDates<TEntity>(DateOnly? startDate, DateOnly? endDate, TimeSpan offset) where TEntity : IBaseEntity
        => entity => (!startDate.HasValue || startDate!.Value.ToDateTimeOffset(offset) <= entity.DateCreated) &&
                     (!endDate.HasValue || entity.DateCreated <= endDate.Value.ToDateTimeOffset(offset));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="offset"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, bool>> BetweenDatesOrLast6Months<TEntity>(DateOnly? startDate, DateOnly? endDate, TimeSpan offset) where TEntity : IBaseEntity
        => !startDate.HasValue || !endDate.HasValue
            ? BetweenDates<TEntity>(DateTimeOffset.UtcNow.AddMonths(-6), DateTimeOffset.UtcNow)
            : BetweenDates<TEntity>(startDate, endDate, offset);
}