using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SocialMediaApp.Application.Core.Abstraction.Http;
using SocialMediaApp.Domain.Core.Extensions;
using SocialMediaApp.Domain.Core.Primitives;
using SocialMediaApp.Domain.Core.Repositories;
using SocialMediaApp.Domain.Core.Specification;
using SocialMediaApp.Domain.Core.Specification.SpecificationMapping;
using SocialMediaApp.Persistence.Context;

namespace SocialMediaApp.Persistence.Repositories;

/// <summary>
/// Generic Repository For All Entities
/// </summary>
public class Repository(ApplicationDbContext context, ILogger<Repository> logger, IHttpService httpService) : IRepository
{
    /// <inheritdoc />
    public async Task<List<TEntity>> QueryAsync<TEntity>(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity
        => await context.Set<TEntity>().AsNoTracking().CreateQuery(specification).AsSplitQuery().ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<List<TResponse>> QueryAsync<TEntity, TResponse>(
        IMappingSpecification<TEntity, TResponse> mappingSpecification,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity
        => await context.Set<TEntity>().AsSplitQuery().AsNoTracking().CreateMappingQuery(mappingSpecification).ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<TResponse> QueryAsync<TEntity, TResponse>(
        Guid id,
        Expression<Func<TEntity, TResponse>> mapping,
        CancellationToken cancellationToken = default) where TEntity : class, IBaseEntity
        => await context.Set<TEntity>().Where(e => e.Id == id).AsNoTracking().Select(mapping).FirstAsync(cancellationToken);

    // <inheritdoc />
    public async Task<TResponse>? QueryAsync<TEntity, TResponse>(
        Expression<Func<TEntity, TResponse>> mapping,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity
        => await filters.Aggregate(context.Set<TEntity>().AsNoTracking().Where(BaseExpressions.FilterDeletedEntity<TEntity>()), Queryable.Where).Select(mapping).FirstOrDefaultAsync(cancellationToken);

    
    /// <inheritdoc />
    public IQueryable<TEntity> TrackingQuery<TEntity>() where TEntity : class, IBaseEntity
        => context.Set<TEntity>().Where(BaseExpressions.FilterDeletedEntity<TEntity>()).AsTracking();
    
    /// <inheritdoc />
    public IQueryable<TEntity> TrackingQueryWithDeleted<TEntity>() where TEntity : class, IBaseEntity
        => context.Set<TEntity>().AsTracking();

    public async Task<TEntity> GetByIdAsync<TEntity>(Guid id, params string[] includeProps) where TEntity : class, IBaseEntity
    {
        var query = context.Set<TEntity>().Where(t => t.Id == id);
        includeProps.Foreach(ip => query = query.Include(ip));
        return await query.FirstAsync();
    }

    public async Task<TEntity?> GetByIdAsyncOrDefault<TEntity>(Guid? id, params string[] includeProps) where TEntity : class, IBaseEntity
    {
        var query = context.Set<TEntity>().Where(t => t.Id == id);
        includeProps.Foreach(ip => query = query.Include(ip));
        return await query.FirstOrDefaultAsync();    
    }

    /// <inheritdoc />
    public IQueryable<TEntity> Query<TEntity>(params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity
        => filters.Aggregate(context.Set<TEntity>().AsNoTracking().Where(BaseExpressions.FilterDeletedEntity<TEntity>()), Queryable.Where);

    /// <inheritdoc />
    public IQueryable<TEntity> QueryWithDeleted<TEntity>(params Expression<Func<TEntity, bool>>[] filters) where TEntity : class, IBaseEntity
        => filters.Aggregate(context.Set<TEntity>().AsNoTracking(), Queryable.Where);

    /// <inheritdoc />
    public void Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        => context.Set<TEntity>().Add(entity);

    /// <inheritdoc />
    public void Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        => context.Set<TEntity>().Update(entity);

    /// <inheritdoc />
    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Publishing Domain Events {DateTimeOffset}", DateTimeOffset.UtcNow);
        context.ChangeTracker.Entries<AggregateRoot>().Foreach(entry =>
        {
            var entity = entry.Entity;
            entity.DomainEvents.Foreach(@event => { });
            entity.ClearDomainEvent();
        });
        context.ChangeTracker.Entries<IBaseEntity>().Foreach(UpdateAuditableEntities);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Remove<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        context.Remove(entity);
    }

    public void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        entity.DateDeleted = System.DateTime.Now;
        context.Set<TEntity>().Update(entity);
    }


    #region HelperMethods

    private void UpdateAuditableEntities(EntityEntry<IBaseEntity> entry)
    {
        var entity = entry.Entity;
        switch (entry)
        {
            case { State: EntityState.Added }:
                entity.DateCreated = DateTimeOffset.UtcNow;
                entity.CreatedBy = httpService.GetCurrentUserId();
                break;
            case { State: EntityState.Modified } when entity.DateDeleted.HasValue:
                entity.DateDeleted = DateTimeOffset.UtcNow;
                entity.DeletedBy = httpService.GetCurrentUserId();
                break;
            case { State: EntityState.Modified }:
                entity.DateUpdated = DateTimeOffset.UtcNow;
                entity.UpdatedBy = httpService.GetCurrentUserId();
                break;
        }
    }

    #endregion
}