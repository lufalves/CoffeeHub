using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.RoastLevel;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class RoastLevelRepository(CoffeeHubDbContext dbContext) : IRoastLevelRepository
{
    public async Task<IReadOnlyList<RoastLevel>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.RoastLevels
            .AsNoTracking()
            .OrderBy(r => r.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RoastLevel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllOrderedAsync(cancellationToken);
    }

    public async Task<RoastLevel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.RoastLevels
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task AddAsync(RoastLevel entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.RoastLevels.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RoastLevel entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.RoastLevels.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.RoastLevels
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
