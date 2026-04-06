using CoffeeHub.Application.Common;
using CoffeeHub.Domain.BrewingMethod;

namespace CoffeeHub.Application.Interfaces;

public interface IBrewingMethodRepository : ICrudRepository<BrewingMethod>
{
    Task<IReadOnlyList<BrewingMethod>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
