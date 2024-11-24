using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification.SpecificationMapping;

public interface IMappingSpecification<TEntity, TDto> : ISpecification<TEntity> where TEntity : IBaseEntity
{
    Expression<Func<TEntity, TDto>> Mapping { get; }
}