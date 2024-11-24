using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Enums;
using SocialMediaApp.Domain.Core.Extensions;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification;

public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : IBaseEntity
{
    /// <inheritdoc />
    public List<Expression<Func<TEntity, bool>>> Criteria { get; private set; } = new();

    /// <inheritdoc />
    public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = new();

    /// <inheritdoc />
    public (Expression<Func<TEntity, object>> Expression, OrderType Type) OrderBy { get; private set; } =
        (Expression: e => e.DateCreated, Type: OrderType.Descending);

    /// <inheritdoc />
    public List<(Expression<Func<TEntity, object>> Expression, OrderType Type)> ThenOrderBy { get; private set; } = new();

    /// <inheritdoc />
    public (int offset, int size)? Pagination { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => Includes.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderBy = (orderByExpression, OrderType.Ascending);

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderBy = (orderByExpression, OrderType.Descending);

    protected void AddThenOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        ThenOrderBy.Add((orderByExpression, OrderType.Ascending));

    protected void AddThenOrderByDescending(Expression<Func<TEntity, object>> orderByExpression)=>
        ThenOrderBy.Add((orderByExpression, OrderType.Descending));
    
    protected void AddFilter(Expression<Func<TEntity, bool>> criteria) => Criteria.Add(criteria);

    protected void AddDateRangeFilter(DateOnly? startDate, DateOnly? endDate, TimeSpan offset) =>
        Criteria.Add(BaseExpressions.BetweenDates<TEntity>(startDate, endDate, offset));

    protected void AddPagination(int pageIndex, int pageSize) => Pagination = (pageIndex, pageSize);
    protected void AddDateDeletedFilter() => Criteria.Add(BaseExpressions.FilterDeletedEntity<TEntity>());
}