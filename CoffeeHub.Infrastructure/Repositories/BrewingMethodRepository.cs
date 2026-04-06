using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Infrastructure.Common;
using CoffeeHub.Infrastructure.Persistence;

namespace CoffeeHub.Infrastructure.Repositories;

public class BrewingMethodRepository(CoffeeHubDbContext dbContext)
    : CrudRepositoryBase<BrewingMethod>(dbContext), IBrewingMethodRepository
{
    protected override IOrderedQueryable<BrewingMethod> ApplyDefaultOrdering(IQueryable<BrewingMethod> query)
    {
        return query.OrderBy(brewingMethod => brewingMethod.Name);
    }

    public Task<IReadOnlyList<BrewingMethod>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return GetAllAsync(cancellationToken);
    }
}
