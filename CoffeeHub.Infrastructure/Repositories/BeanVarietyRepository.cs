using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BeanVariety;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class BeanVarietyRepository(CoffeeHubDbContext dbContext) : IBeanVarietyRepository
{
    public async Task<IReadOnlyList<BeanVariety>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.BeanVarieties
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<BeanVariety?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.BeanVarieties
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(BeanVariety entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.BeanVarieties.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(BeanVariety entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.BeanVarieties.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.BeanVarieties
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
