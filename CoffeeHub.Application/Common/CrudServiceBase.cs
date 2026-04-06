using CoffeeHub.Domain.Common;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Common;

public abstract class CrudServiceBase<TEntity, TRepository>(
    TRepository repository,
    ILogger logger)
    where TEntity : EntityBase
    where TRepository : ICrudRepository<TEntity>
{
    protected TRepository Repository { get; } = repository;
    protected ILogger Logger { get; } = logger;

    protected abstract string EntityName { get; }

    public virtual Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Repository.GetAllAsync(cancellationToken);
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(id, nameof(id), $"{EntityName} id");
        return Repository.GetByIdAsync(id, cancellationToken);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ValidateForSave(entity);
        NormalizeForSave(entity);

        await Repository.AddAsync(entity, cancellationToken);
        Logger.LogInformation("{EntityName} created: {EntityId}", EntityName, entity.Id);
        return entity;
    }

    public virtual async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        EntityValidator.ThrowIfEmptyGuid(entity.Id, nameof(entity), $"{EntityName} id");
        ValidateForSave(entity);
        NormalizeForSave(entity);

        var existingEntity = await Repository.GetByIdAsync(entity.Id, cancellationToken);

        if (existingEntity is null)
        {
            Logger.LogWarning("Attempted to update non-existent {EntityName}: {EntityId}", EntityName, entity.Id);
            return null;
        }

        ApplyUpdates(existingEntity, entity);

        await Repository.UpdateAsync(existingEntity, cancellationToken);
        Logger.LogInformation("{EntityName} updated: {EntityId}", EntityName, entity.Id);
        return existingEntity;
    }

    public virtual async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(id, nameof(id), $"{EntityName} id");

        var existingEntity = await Repository.GetByIdAsync(id, cancellationToken);

        if (existingEntity is null)
        {
            Logger.LogWarning("Attempted to delete non-existent {EntityName}: {EntityId}", EntityName, id);
            return false;
        }

        await Repository.SoftDeleteAsync(id, cancellationToken);
        Logger.LogInformation("{EntityName} soft-deleted: {EntityId}", EntityName, id);
        return true;
    }

    protected abstract void ValidateForSave(TEntity entity);
    protected abstract void NormalizeForSave(TEntity entity);
    protected abstract void ApplyUpdates(TEntity target, TEntity source);
}
