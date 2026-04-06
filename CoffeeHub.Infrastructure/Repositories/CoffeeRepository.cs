using CoffeeHub.Application.Common;
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
            .OrderBy(coffee => coffee.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .FirstOrDefaultAsync(coffee => coffee.Id == id, cancellationToken);
    }

    public async Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .FirstOrDefaultAsync(coffee => coffee.Barcode == barcode, cancellationToken);
    }

    public async Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees
            .AsNoTracking()
            .Include(c => c.Roastery)
            .Include(c => c.Origin)
            .Include(c => c.Farm)
            .Include(c => c.BeanVariety)
            .Include(c => c.RoastLevel)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
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
        var coffee = await dbContext.Coffees
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (coffee is null || coffee.IsDeleted)
        {
            return;
        }

        coffee.IsDeleted = true;
        coffee.DeletedAt = DateTimeOffset.UtcNow;
        coffee.UpdatedAt = coffee.DeletedAt.Value;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Coffees.CountAsync(cancellationToken);
    }

    public async Task<PagedResult<Coffee>> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roasteryId = null,
        Guid? originId = null,
        Guid? roastLevelId = null,
        Guid? beanVarietyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Coffees.AsQueryable();

        if (roasteryId.HasValue)
            query = query.Where(c => c.RoasteryId == roasteryId.Value);
        if (originId.HasValue)
            query = query.Where(c => c.OriginId == originId.Value);
        if (roastLevelId.HasValue)
            query = query.Where(c => c.RoastLevelId == roastLevelId.Value);
        if (beanVarietyId.HasValue)
            query = query.Where(c => c.BeanVarietyId == beanVarietyId.Value);
        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Barcode.Contains(searchTerm));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Coffee>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await GetPagedAsync(page, pageSize, roasteryId: roasteryId, cancellationToken: cancellationToken);
    }

    public async Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await GetPagedAsync(page, pageSize, originId: originId, cancellationToken: cancellationToken);
    }
}
