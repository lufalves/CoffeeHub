using CoffeeHub.Application.Common;
using CoffeeHub.Application.Services;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Tests.Common;
using CoffeeHub.Tests.Fakes;

namespace CoffeeHub.Tests;

public class CoffeeServiceTests : ServiceTestBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateCoffee_WhenDataIsValid()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Ethiopia Yirgacheffe",
            RoasteryId = Guid.NewGuid(),
            Description = "Floral and citrus"
        };

        var createdCoffee = await service.CreateAsync(coffee, TestContext.Current.CancellationToken);

        Assert.Same(coffee, createdCoffee);
        Assert.Same(coffee, repository.AddedCoffee);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenRoasteryIdIsEmpty()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Invalid Coffee",
            RoasteryId = Guid.Empty
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(coffee, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenCoffeeDoesNotExist()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var result = await service.UpdateAsync(new Coffee
        {
            Id = Guid.NewGuid(),
            Barcode = "7891000100103",
            Name = "Coffee",
            RoasteryId = Guid.NewGuid()
        }, TestContext.Current.CancellationToken);

        Assert.Null(result);
        Assert.Null(repository.UpdatedCoffee);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCoffee_WhenCoffeeExists()
    {
        var existingCoffee = new Coffee
        {
            Id = Guid.NewGuid(),
            Barcode = "7891000100103",
            Name = "Old Coffee",
            RoasteryId = Guid.NewGuid()
        };

        var repository = new FakeCoffeeRepository
        {
            CoffeeById = existingCoffee
        };

        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var result = await service.UpdateAsync(new Coffee
        {
            Id = existingCoffee.Id,
            Barcode = existingCoffee.Barcode,
            Name = "New Coffee",
            RoasteryId = existingCoffee.RoasteryId,
            Description = "Updated description"
        }, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("New Coffee", existingCoffee.Name);
        Assert.Equal("Updated description", existingCoffee.Description);
        Assert.Equal(existingCoffee, repository.UpdatedCoffee);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldReturnFalse_WhenCoffeeDoesNotExist()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var result = await service.SoftDeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.False(result);
        Assert.Null(repository.SoftDeletedCoffeeId);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldDelete_WhenCoffeeExists()
    {
        var existingCoffee = new Coffee
        {
            Id = Guid.NewGuid(),
            Barcode = "7891000100103",
            Name = "Coffee",
            RoasteryId = Guid.NewGuid()
        };

        var repository = new FakeCoffeeRepository
        {
            CoffeeById = existingCoffee
        };

        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var result = await service.SoftDeleteAsync(existingCoffee.Id, TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Equal(existingCoffee.Id, repository.SoftDeletedCoffeeId);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenBarcodeAlreadyExists()
    {
        var repository = new FakeCoffeeRepository
        {
            CoffeeByBarcode = new Coffee
            {
                Id = Guid.NewGuid(),
                Barcode = "7891000100103",
                Name = "Existing Coffee",
                RoasteryId = Guid.NewGuid()
            }
        };

        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Duplicate Coffee",
            RoasteryId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(coffee, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenBarcodeIsEmpty()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, Logger<CoffeeService>());

        var coffee = new Coffee
        {
            Barcode = string.Empty,
            Name = "Coffee",
            RoasteryId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(coffee, TestContext.Current.CancellationToken));
    }

}
