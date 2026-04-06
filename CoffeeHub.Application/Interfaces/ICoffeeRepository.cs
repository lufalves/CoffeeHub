using CoffeeHub.Application.Common;
using CoffeeHub.Domain.Coffee;

namespace CoffeeHub.Application.Interfaces;

public interface ICoffeeRepository
{
    Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roasteryId = null,
        Guid? originId = null,
        Guid? roastLevelId = null,
        Guid? beanVarietyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
