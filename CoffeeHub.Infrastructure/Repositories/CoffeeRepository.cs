using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class CoffeeRepository(CoffeeHubDbContext dbContext) : ICoffeeRepository
{
    public async Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .Where(coffee => !coffee.IsDeleted)
            .OrderBy(coffee => coffee.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .FirstOrDefaultAsync(coffee => coffee.Id == id && !coffee.IsDeleted, cancellationToken);
    }

    public async Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .FirstOrDefaultAsync(coffee => coffee.Barcode == barcode && !coffee.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        if (coffee.Id == Guid.Empty)
        {
            coffee.Id = Guid.NewGuid();
        }

        var now = DateTimeOffset.UtcNow;
        coffee.CreatedAt = now;
        coffee.UpdatedAt = now;

        dbContext.Coffees.Add(coffee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        coffee.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.Coffees.Update(coffee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var coffee = await dbContext.Coffees.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (coffee is null || coffee.IsDeleted)
        {
            return;
        }

        coffee.IsDeleted = true;
        coffee.DeletedAt = DateTimeOffset.UtcNow;
        coffee.UpdatedAt = coffee.DeletedAt.Value;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
