using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.CoffeeShop;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class CoffeeShopRepository(CoffeeHubDbContext dbContext) : ICoffeeShopRepository
{
    public async Task<IReadOnlyList<CoffeeShop>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.CoffeeShops
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<CoffeeShop?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.CoffeeShops
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(CoffeeShop entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.CoffeeShops.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CoffeeShop entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.CoffeeShops.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.CoffeeShops
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
