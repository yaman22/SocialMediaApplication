using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Enums;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification;

public interface ISpecification<TEntity> where TEntity : IBaseEntity
{
    /// <summary>
    /// List Of Expressions that represent Query filters
    /// </summary>
    List<Expression<Func<TEntity, bool>>> Criteria { get; }

    /// <summary>
    /// List of join tables
    /// </summary>
    List<Expression<Func<TEntity, object>>> Includes { get; }

    /// <summary>
    /// Column The query will order and the order type
    /// </summary>
    (Expression<Func<TEntity, object>> Expression, OrderType Type) OrderBy { get; }

    /// <summary>
    /// Other Column To OrderBy
    /// </summary>
    List<(Expression<Func<TEntity, object>> Expression, OrderType Type)> ThenOrderBy { get; }

    /// <summary>
    /// OffsetPagination
    /// </summary>
    public (int offset, int size)? Pagination { get; }
}