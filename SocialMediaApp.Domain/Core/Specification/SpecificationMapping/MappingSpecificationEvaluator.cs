using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Domain.Core.Specification.SpecificationMapping;

public static class MappingSpecificationEvaluator
{
    public static IQueryable<TDto> CreateMappingQuery<TEntity, TDto>(
        this IQueryable<TEntity> queryable,
        IMappingSpecification<TEntity, TDto> mappingSpecification)
        where TEntity : class, IBaseEntity
        => queryable.CreateQuery(mappingSpecification).Select(mappingSpecification.Mapping);
}