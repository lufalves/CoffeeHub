using CoffeeHub.Application.Common;
using CoffeeHub.Domain.Common;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Common;

public abstract class CrudRepositoryBase<TEntity>(CoffeeHubDbContext dbContext) : ICrudRepository<TEntity>
    where TEntity : EntityBase
{
    protected CoffeeHubDbContext DbContext { get; } = dbContext;

    protected virtual IQueryable<TEntity> BuildGetAllQuery()
    {
        return DbContext.Set<TEntity>().AsNoTracking();
    }

    protected virtual IQueryable<TEntity> BuildGetByIdQuery()
    {
        return DbContext.Set<TEntity>().AsNoTracking();
    }

    protected virtual IOrderedQueryable<TEntity> ApplyDefaultOrdering(IQueryable<TEntity> query)
    {
        return query.OrderBy(entity => entity.CreatedAt);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ApplyDefaultOrdering(BuildGetAllQuery())
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await BuildGetByIdQuery()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        var now = DateTimeOffset.UtcNow;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;

        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await DbContext.Set<TEntity>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            return;
        }

        entity.IsDeleted = true;
        entity.DeletedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = entity.DeletedAt.Value;

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
