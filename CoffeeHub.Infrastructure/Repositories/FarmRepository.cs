using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Farm;
using CoffeeHub.Infrastructure.Common;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class FarmRepository(CoffeeHubDbContext dbContext)
    : CrudRepositoryBase<Farm>(dbContext), IFarmRepository
{
    protected override IOrderedQueryable<Farm> ApplyDefaultOrdering(IQueryable<Farm> query)
    {
        return query.OrderBy(farm => farm.Name);
    }

    public async Task<IReadOnlyList<Farm>> GetByOriginIdAsync(Guid originId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Farms
            .AsNoTracking()
            .Where(f => f.OriginId == originId)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);
    }
}
