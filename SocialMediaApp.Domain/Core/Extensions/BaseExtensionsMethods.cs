using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Extensions;

public static class BaseExtensionsMethods
{
    public static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize) where TEntity : class, IBaseEntity
        => query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

    public static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> query, Guid? nextCursor, int pageSize) where TEntity : class, IBaseEntity
        => query.Where(e => !nextCursor.HasValue || e.Id >= nextCursor).Take(pageSize);

    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity
        => filters.Aggregate(query, Queryable.Where);
}