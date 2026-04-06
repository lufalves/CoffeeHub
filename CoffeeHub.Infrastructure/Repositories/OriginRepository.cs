using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Origin;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class OriginRepository(CoffeeHubDbContext dbContext) : IOriginRepository
{
    public async Task<IReadOnlyList<Origin>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Origins
            .AsNoTracking()
            .OrderBy(o => o.Country)
            .ToListAsync(cancellationToken);
    }

    public async Task<Origin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Origins
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task AddAsync(Origin entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.Origins.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Origin entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.Origins.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Origins
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
