using CoffeeHub.Application.Common;
using CoffeeHub.Domain.Coffee;

namespace CoffeeHub.Application.Interfaces;

public interface ICoffeeService
{
    Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roasteryId = null,
        Guid? originId = null,
        Guid? roastLevelId = null,
        Guid? beanVarietyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Coffee> CreateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task<Coffee?> UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default);
}
