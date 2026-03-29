using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;

namespace CoffeeHub.Application.Services;

public class CoffeeService(ICoffeeRepository coffeeRepository) : ICoffeeService
{
    public Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return coffeeRepository.GetAllAsync(cancellationToken);
    }

    public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Coffee id must be informed.", nameof(id));
        }

        return coffeeRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Coffee> CreateAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        ValidateCoffee(coffee);

        var normalizedBarcode = NormalizeBarcode(coffee.Barcode);
        var existingCoffee = await coffeeRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);

        if (existingCoffee is not null)
        {
            throw new InvalidOperationException("A coffee with this barcode already exists.");
        }

        coffee.Barcode = normalizedBarcode;

        await coffeeRepository.AddAsync(coffee, cancellationToken);
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
            return null;
        }

        var normalizedBarcode = NormalizeBarcode(coffee.Barcode);
        var coffeeWithSameBarcode = await coffeeRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);

        if (coffeeWithSameBarcode is not null && coffeeWithSameBarcode.Id != coffee.Id)
        {
            throw new InvalidOperationException("A coffee with this barcode already exists.");
        }

        existingCoffee.Barcode = normalizedBarcode;
        existingCoffee.Name = coffee.Name;
        existingCoffee.Description = coffee.Description;
        existingCoffee.RoasteryId = coffee.RoasteryId;
        existingCoffee.OriginId = coffee.OriginId;
        existingCoffee.FarmId = coffee.FarmId;
        existingCoffee.BeanVarietyId = coffee.BeanVarietyId;
        existingCoffee.RoastLevelId = coffee.RoastLevelId;

        await coffeeRepository.UpdateAsync(existingCoffee, cancellationToken);

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
            return false;
        }

        await coffeeRepository.SoftDeleteAsync(id, cancellationToken);
        return true;
    }

    private static void ValidateCoffee(Coffee coffee)
    {
        ArgumentNullException.ThrowIfNull(coffee);

        if (string.IsNullOrWhiteSpace(coffee.Barcode))
        {
            throw new ArgumentException("Coffee barcode must be informed.", nameof(coffee));
        }

        if (NormalizeBarcode(coffee.Barcode).Length > 50)
        {
            throw new ArgumentException("Coffee barcode cannot exceed 50 characters.", nameof(coffee));
        }

        if (string.IsNullOrWhiteSpace(coffee.Name))
        {
            throw new ArgumentException("Coffee name must be informed.", nameof(coffee));
        }

        if (coffee.Name.Length > 200)
        {
            throw new ArgumentException("Coffee name cannot exceed 200 characters.", nameof(coffee));
        }

        if (coffee.Description?.Length > 2000)
        {
            throw new ArgumentException("Coffee description cannot exceed 2000 characters.", nameof(coffee));
        }

        if (coffee.RoasteryId == Guid.Empty)
        {
            throw new ArgumentException("Roastery id must be informed.", nameof(coffee));
        }
    }

    private static string NormalizeBarcode(string barcode)
    {
        return barcode.Trim();
    }
}
