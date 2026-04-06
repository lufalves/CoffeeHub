using CoffeeHub.Domain.Coffee;
using CoffeeHub.Application.Common;
using Microsoft.Extensions.Logging;
using CoffeeHub.Application.Interfaces;

namespace CoffeeHub.Application.Services;

public class CoffeeService(ICoffeeRepository coffeeRepository, ILogger<CoffeeService> logger) : ICoffeeService
{
    public Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetAllAsync(cancellationToken);
    }

    public Task<PagedResult<Coffee>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetPagedAsync(page, pageSize, cancellationToken: cancellationToken);
    }

    public Task<PagedResult<Coffee>> GetPagedAsync(
        int page,
        int pageSize,
        Guid? roasteryId = null,
        Guid? originId = null,
        Guid? roastLevelId = null,
        Guid? beanVarietyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetPagedAsync(page, pageSize, roasteryId, originId, roastLevelId, beanVarietyId, searchTerm, cancellationToken);
    }

    public Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetByRoasteryIdAsync(roasteryId, page, pageSize, cancellationToken);
    }

    public Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetByOriginIdAsync(originId, page, pageSize, cancellationToken);
    }

    public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Coffee id must be informed.", nameof(id));
        }

        return coffeeRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Coffee id must be informed.", nameof(id));
        }

        return coffeeRepository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public async Task<Coffee> CreateAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        ValidateCoffee(coffee);

        var normalizedBarcode = EntityValidator.NormalizeBarcode(coffee.Barcode);
        var existingCoffee = await coffeeRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);

        if (existingCoffee is not null)
        {
            logger.LogWarning("Attempted to create coffee with duplicate barcode: {Barcode}", coffee.Barcode);
            throw new InvalidOperationException("A coffee with this barcode already exists.");
        }

        coffee.Barcode = normalizedBarcode;

        await coffeeRepository.AddAsync(coffee, cancellationToken);
        logger.LogInformation("Coffee created: {CoffeeId} - {Barcode}", coffee.Id, coffee.Barcode);
        return coffee;
    }

    public async Task<Coffee?> UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        if (coffee.Id == Guid.Empty)
        {
            throw new ArgumentException("Coffee id must be informed.", nameof(coffee));
        }

        ValidateCoffee(coffee);

        var existingCoffee = await coffeeRepository.GetByIdAsync(coffee.Id, cancellationToken);

        if (existingCoffee is null)
        {
            logger.LogWarning("Attempted to update non-existent coffee: {CoffeeId}", coffee.Id);
            return null;
        }

        var normalizedBarcode = EntityValidator.NormalizeBarcode(coffee.Barcode);
        var coffeeWithSameBarcode = await coffeeRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);

        if (coffeeWithSameBarcode is not null && coffeeWithSameBarcode.Id != coffee.Id)
        {
            logger.LogWarning("Attempted to update coffee with duplicate barcode: {Barcode}", coffee.Barcode);
            throw new InvalidOperationException("A coffee with this barcode already exists.");
        }

        existingCoffee.Barcode = normalizedBarcode;
        existingCoffee.Name = EntityValidator.NormalizeName(coffee.Name);
        existingCoffee.Description = EntityValidator.NormalizeOptionalString(coffee.Description);
        existingCoffee.RoasteryId = coffee.RoasteryId;
        existingCoffee.OriginId = coffee.OriginId;
        existingCoffee.FarmId = coffee.FarmId;
        existingCoffee.BeanVarietyId = coffee.BeanVarietyId;
        existingCoffee.RoastLevelId = coffee.RoastLevelId;

        await coffeeRepository.UpdateAsync(existingCoffee, cancellationToken);
        logger.LogInformation("Coffee updated: {CoffeeId}", coffee.Id);
        return existingCoffee;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Coffee id must be informed.", nameof(id));
        }

        var existingCoffee = await coffeeRepository.GetByIdAsync(id, cancellationToken);

        if (existingCoffee is null)
        {
            logger.LogWarning("Attempted to delete non-existent coffee: {CoffeeId}", id);
            return false;
        }

        await coffeeRepository.SoftDeleteAsync(id, cancellationToken);
        logger.LogInformation("Coffee soft-deleted: {CoffeeId}", id);
        return true;
    }

    private static void ValidateCoffee(Coffee coffee)
    {
        ArgumentNullException.ThrowIfNull(coffee);

        EntityValidator.ThrowIfNullOrWhiteSpace(coffee.Barcode, nameof(coffee), "Coffee barcode");
        EntityValidator.ThrowIfExceedsLength(coffee.Barcode, 50, nameof(coffee), "Coffee barcode");
        EntityValidator.ThrowIfNullOrWhiteSpace(coffee.Name, nameof(coffee), "Coffee name");
        EntityValidator.ThrowIfExceedsLength(coffee.Name, 200, nameof(coffee), "Coffee name");
        EntityValidator.ThrowIfExceedsLength(coffee.Description, 2000, nameof(coffee), "Coffee description");
        EntityValidator.ThrowIfEmptyGuid(coffee.RoasteryId, nameof(coffee), "Roastery id");
    }
}
