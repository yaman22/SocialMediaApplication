using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification.SpecificationMapping;

public class MappingSpecification<TEntity, TResponse> : Specification<TEntity>, IMappingSpecification<TEntity, TResponse> where TEntity : IBaseEntity
{
    public Expression<Func<TEntity, TResponse>> Mapping { get; private set; }
    protected void ApplyMapping(Expression<Func<TEntity, TResponse>> mapping) => Mapping = mapping;
}