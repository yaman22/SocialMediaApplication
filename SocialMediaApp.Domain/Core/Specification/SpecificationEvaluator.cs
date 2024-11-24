using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Domain.Core.Enums;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification;

public static class SpecificationEvaluator
{
    /// <summary>
    /// Create The Query From Specification
    /// </summary>
    /// <param name="queryable">The query will be executed into database</param>
    /// <param name="specification">specification</param>
    /// <typeparam name="TEntity">Database Table</typeparam>
    /// <returns></returns>
    public static IQueryable<TEntity> CreateQuery<TEntity>(this IQueryable<TEntity> queryable, ISpecification<TEntity> specification) where TEntity : class, IBaseEntity
    {
        //Apply includes To join tables
        var query = specification.Includes.Aggregate(queryable, (current, includeExpression) => current.Include(includeExpression));

        //Apply Filters 
        query = specification.Criteria.Aggregate(query, Queryable.Where);

        //Apply order
        var (orderByExpression, orderType) = specification.OrderBy;
        var orderQuery = orderType == OrderType.Ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression)/* query as IOrderedQueryable<TEntity>*/;
        
        //Apply ThenOrderBy
        specification.ThenOrderBy.ForEach(tuple =>
        {
            var (expression, type) = tuple;
            orderQuery = type == OrderType.Ascending ? orderQuery.ThenBy(expression) : orderQuery.ThenByDescending(expression);
        });
        
        //Apply Paging
        if (specification.Pagination.HasValue)
        {
            query = orderQuery;
            var (offset, size) = specification.Pagination.Value;
            query = query.Skip((offset - 1) * size).Take(size);
        }
        return query;
    }
}