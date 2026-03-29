using CoffeeHub.Domain.Coffee;

namespace CoffeeHub.Application.Interfaces;

public interface ICoffeeService
{
    Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coffee> CreateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task<Coffee?> UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
