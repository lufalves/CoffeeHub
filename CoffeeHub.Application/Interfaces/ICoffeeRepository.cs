using CoffeeHub.Domain.Coffee;

namespace CoffeeHub.Application.Interfaces;

public interface ICoffeeRepository
{
    Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task AddAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
