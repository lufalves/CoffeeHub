using CoffeeHub.Application.Common;
using CoffeeHub.Domain.Farm;

namespace CoffeeHub.Application.Interfaces;

public interface IFarmService : ICrudService<Farm>
{
    Task<IReadOnlyList<Farm>> GetByOriginIdAsync(Guid originId, CancellationToken cancellationToken = default);
}
