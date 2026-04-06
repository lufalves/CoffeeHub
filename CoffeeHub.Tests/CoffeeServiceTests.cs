using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Application.Services;
using CoffeeHub.Domain.Coffee;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoffeeHub.Tests;

public class CoffeeServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateCoffee_WhenDataIsValid()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Ethiopia Yirgacheffe",
            RoasteryId = Guid.NewGuid(),
            Description = "Floral and citrus"
        };

        var createdCoffee = await service.CreateAsync(coffee);

        Assert.Same(coffee, createdCoffee);
        Assert.Same(coffee, repository.AddedCoffee);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenRoasteryIdIsEmpty()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Invalid Coffee",
            RoasteryId = Guid.Empty
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(coffee));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenCoffeeDoesNotExist()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var result = await service.UpdateAsync(new Coffee
        {
            Id = Guid.NewGuid(),
            Barcode = "7891000100103",
            Name = "Coffee",
            RoasteryId = Guid.NewGuid()
        });

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

        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var result = await service.UpdateAsync(new Coffee
        {
            Id = existingCoffee.Id,
            Barcode = existingCoffee.Barcode,
            Name = "New Coffee",
            RoasteryId = existingCoffee.RoasteryId,
            Description = "Updated description"
        });

        Assert.NotNull(result);
        Assert.Equal("New Coffee", existingCoffee.Name);
        Assert.Equal("Updated description", existingCoffee.Description);
        Assert.Equal(existingCoffee, repository.UpdatedCoffee);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldReturnFalse_WhenCoffeeDoesNotExist()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var result = await service.SoftDeleteAsync(Guid.NewGuid());

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

        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var result = await service.SoftDeleteAsync(existingCoffee.Id);

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

        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var coffee = new Coffee
        {
            Barcode = "7891000100103",
            Name = "Duplicate Coffee",
            RoasteryId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(coffee));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenBarcodeIsEmpty()
    {
        var repository = new FakeCoffeeRepository();
        var service = new CoffeeService(repository, NullLogger<CoffeeService>.Instance);

        var coffee = new Coffee
        {
            Barcode = string.Empty,
            Name = "Coffee",
            RoasteryId = Guid.NewGuid()
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(coffee));
    }

    private sealed class FakeCoffeeRepository : ICoffeeRepository
    {
        public Coffee? CoffeeById { get; set; }
        public Coffee? CoffeeByBarcode { get; set; }
        public Coffee? AddedCoffee { get; private set; }
        public Coffee? UpdatedCoffee { get; private set; }
        public Guid? SoftDeletedCoffeeId { get; private set; }

        public Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<Coffee>>(Array.Empty<Coffee>());
        }

        public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CoffeeById?.Id == id ? CoffeeById : null);
        }

        public Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CoffeeByBarcode?.Barcode == barcode ? CoffeeByBarcode : null);
        }

        public Task AddAsync(Coffee coffee, CancellationToken cancellationToken = default)
        {
            AddedCoffee = coffee;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Coffee coffee, CancellationToken cancellationToken = default)
        {
            UpdatedCoffee = coffee;
            return Task.CompletedTask;
        }

        public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            SoftDeletedCoffeeId = id;
            return Task.CompletedTask;
        }

        public Task<PagedResult<Coffee>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PagedResult<Coffee>());
        }

        public Task<PagedResult<Coffee>> GetPagedAsync(int page, int pageSize, Guid? roasteryId = null, Guid? originId = null, Guid? roastLevelId = null, Guid? beanVarietyId = null, string? searchTerm = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PagedResult<Coffee>());
        }

        public Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CoffeeById?.Id == id ? CoffeeById : null);
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PagedResult<Coffee>());
        }

        public Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PagedResult<Coffee>());
        }
    }
}
