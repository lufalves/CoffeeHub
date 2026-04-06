using CoffeeHub.Application.Common;
using CoffeeHub.Domain.RoastLevel;

namespace CoffeeHub.Application.Interfaces;

public interface IRoastLevelService : ICrudService<RoastLevel>
{
    Task<IReadOnlyList<RoastLevel>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}
