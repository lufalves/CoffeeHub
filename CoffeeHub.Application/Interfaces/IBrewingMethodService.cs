using CoffeeHub.Application.Common;
using CoffeeHub.Domain.BrewingMethod;

namespace CoffeeHub.Application.Interfaces;

public interface IBrewingMethodService : ICrudService<BrewingMethod>
{
    Task<IReadOnlyList<BrewingMethod>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
