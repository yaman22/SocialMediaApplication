using System.Linq.Expressions;
using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Core.Specification;
using SocialMediaApp.Domain.Core.Specification.SpecificationMapping;

namespace SocialMediaApp.Domain.Core.Repositories;

public interface IRepository
{
    /// <summary>
    /// Query With specification 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity">Entity that represent a table in database</typeparam>
    /// <returns>List of Entity</returns>
    Task<List<TEntity>> QueryAsync<TEntity>(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="mappingSpecification"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    Task<List<TResponse>> QueryAsync<TEntity, TResponse>(
        IMappingSpecification<TEntity, TResponse> mappingSpecification,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity;

    /// <summary>
    /// Query with specific Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="mapping"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity">Entity that represent a table in database</typeparam>
    /// <typeparam name="TResponse">Dto that represent a response</typeparam>
    /// <returns>List of Dto Response</returns>
    Task<TResponse> QueryAsync<TEntity, TResponse>(
        Guid id,
        Expression<Func<TEntity, TResponse>> mapping,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity;
    
    /// <summary>
    /// Query With No tracking From DataBase With Specific Array of Filters And Date Deleted Filters
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="cancellationToken"></param>
    /// /// <param name="filters">Array of expression filters</param>
    /// <typeparam name="TEntity">Entity that represent a table in database</typeparam>
    /// <typeparam name="TResponse">Dto that represent a response</typeparam>
    /// <returns>List of Dto Response</returns>
    Task<TResponse> QueryAsync<TEntity, TResponse>(
        Expression<Func<TEntity, TResponse>> mapping,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity;

    /// <summary>
    /// Query With No tracking From DataBase With DateDeleted Filter
    /// </summary>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    /// <returns>An IQueryable Of tracking Filtered TEntity</returns>
    IQueryable<TEntity> TrackingQuery<TEntity>() where TEntity : class, IBaseEntity;

    /// <summary>
    /// Query With No tracking From DataBase With DateDeleted Filter
    /// </summary>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    /// <returns>An IQueryable Of tracking Filtered TEntity</returns>
    IQueryable<TEntity> TrackingQueryWithDeleted<TEntity>() where TEntity : class, IBaseEntity;

    /// <summary>
    /// Query With No tracking From DataBase With DateDeleted Filter
    /// </summary>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    /// <returns>An IQueryable Of tracking Filtered TEntity</returns>
    Task<TEntity> GetByIdAsync<TEntity>(Guid id, params string[] includeProps) where TEntity : class, IBaseEntity;

    Task<TEntity?> GetByIdAsyncOrDefault<TEntity>(Guid? id, params string[] includeProps) where TEntity : class, IBaseEntity;
    /// <summary>
    /// Query With No tracking From DataBase With Specific Array of Filters And Date Deleted Filters
    /// </summary>
    /// <param name="filters">Array of expression filters</param>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    /// <returns>An IQueryable Of un tracking Filtered TEntity</returns>
    IQueryable<TEntity> Query<TEntity>(params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity;

    /// <summary>
    /// Query With No tracking From DataBase With Specific Array of Filters And Date Deleted Filters
    /// </summary>
    /// <param name="filters">Filters</param>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    /// <returns>An IQueryable Of NoTracking Filtered TEntity</returns>
    IQueryable<TEntity> QueryWithDeleted<TEntity>(params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity;

    /// <summary>
    /// Add record to database Table
    /// </summary>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    void Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

    /// <summary>
    /// Update record in database Table
    /// </summary>
    /// <typeparam name="TEntity">Entity that Represent a table on database</typeparam>
    void Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

    /// <summary>
    /// publish your domainEvent and persist your changes
    /// </summary>
    /// <param name="cancellationToken">to cancel</param>
    /// <returns>number of record that effected</returns>
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

    void Remove<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

    void SoftDelete<TEntity1>(TEntity1 entity) where TEntity1 : class, IBaseEntity;
}