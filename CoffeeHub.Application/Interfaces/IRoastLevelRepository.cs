using CoffeeHub.Application.Common;
using CoffeeHub.Domain.RoastLevel;

namespace CoffeeHub.Application.Interfaces;

public interface IRoastLevelRepository : ICrudRepository<RoastLevel>
{
    Task<IReadOnlyList<RoastLevel>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
