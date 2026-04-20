using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.Recipe;
using CoffeeHub.Domain.Review;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Tests.Fakes;

public sealed class FakeAuthUserRepository : IUserRepository
{
    public List<User> Users { get; } = [];
    public User? UpdatedUser { get; private set; }

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<User>>(Users.ToList());
    }

    public Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = Users
            .OrderBy(item => item.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<User>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = Users.Count
        });
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.FirstOrDefault(item => item.Id == id));
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.FirstOrDefault(item => item.Email == email));
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }

        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        UpdatedUser = user;

        var index = Users.FindIndex(item => item.Id == user.Id);

        if (index >= 0)
        {
            Users[index] = user;
        }

        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = Users.FirstOrDefault(item => item.Id == id);

        if (user is not null)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTimeOffset.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Users.Count);
    }
}

public sealed class FakeRecipeRepository : IRecipeRepository
{
    public List<Recipe> Recipes { get; } = [];
    public Guid? SoftDeletedRecipeId { get; private set; }

    public Task<IReadOnlyList<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Recipe>>(Recipes.Where(item => !item.IsDeleted).ToList());
    }

    public Task<IReadOnlyList<Recipe>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Recipe>>(Recipes.Where(item => !item.IsDeleted && item.UserId == userId).ToList());
    }

    public Task<IReadOnlyList<Recipe>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Recipe>>(Recipes.Where(item => !item.IsDeleted && item.CoffeeId == coffeeId).ToList());
    }

    public Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Recipes.FirstOrDefault(item => !item.IsDeleted && item.Id == id));
    }

    public Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        if (recipe.Id == Guid.Empty)
        {
            recipe.Id = Guid.NewGuid();
        }

        Recipes.Add(recipe);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        var index = Recipes.FindIndex(item => item.Id == recipe.Id);

        if (index >= 0)
        {
            Recipes[index] = recipe;
        }

        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recipe = Recipes.FirstOrDefault(item => item.Id == id);

        if (recipe is not null)
        {
            recipe.IsDeleted = true;
            recipe.DeletedAt = DateTimeOffset.UtcNow;
            SoftDeletedRecipeId = id;
        }

        return Task.CompletedTask;
    }
}

public sealed class FakeReviewRepository : IReviewRepository
{
    public List<Review> Reviews { get; } = [];
    public Guid? SoftDeletedReviewId { get; private set; }

    public Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Review>>(Reviews.Where(item => !item.IsDeleted).ToList());
    }

    public Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Review>>(Reviews.Where(item => !item.IsDeleted && item.UserId == userId).ToList());
    }

    public Task<IReadOnlyList<Review>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Review>>(Reviews.Where(item => !item.IsDeleted && item.CoffeeId == coffeeId).ToList());
    }

    public Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Reviews.FirstOrDefault(item => !item.IsDeleted && item.Id == id));
    }

    public Task<decimal?> GetAverageRatingAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        var values = Reviews
            .Where(item => !item.IsDeleted && item.CoffeeId == coffeeId)
            .Select(item => item.Rating)
            .ToList();

        return Task.FromResult(values.Count == 0 ? (decimal?)null : values.Average());
    }

    public Task AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        if (review.Id == Guid.Empty)
        {
            review.Id = Guid.NewGuid();
        }

        Reviews.Add(review);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Review review, CancellationToken cancellationToken = default)
    {
        var index = Reviews.FindIndex(item => item.Id == review.Id);

        if (index >= 0)
        {
            Reviews[index] = review;
        }

        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var review = Reviews.FirstOrDefault(item => item.Id == id);

        if (review is not null)
        {
            review.IsDeleted = true;
            review.DeletedAt = DateTimeOffset.UtcNow;
            SoftDeletedReviewId = id;
        }

        return Task.CompletedTask;
    }
}

public sealed class FakePasswordHashService : IPasswordHashService
{
    public string HashPassword(User user, string password)
    {
        return $"hashed:{password}";
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        return string.Equals(hashedPassword, $"hashed:{providedPassword}", StringComparison.Ordinal);
    }
}

public sealed class FakeCoffeeRepository : ICoffeeRepository
{
    public List<Coffee> Coffees { get; } = [];
    public Coffee? CoffeeById { get; set; }
    public Coffee? CoffeeByBarcode { get; set; }
    public Coffee? AddedCoffee { get; private set; }
    public Coffee? UpdatedCoffee { get; private set; }
    public Guid? SoftDeletedCoffeeId { get; private set; }

    public Task<IReadOnlyList<Coffee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Coffee>>(Coffees.Where(c => !c.IsDeleted).ToList());
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
        return Task.FromResult(new PagedResult<Coffee>());
    }

    public Task<PagedResult<Coffee>> GetByRoasteryIdAsync(Guid roasteryId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PagedResult<Coffee>());
    }

    public Task<PagedResult<Coffee>> GetByOriginIdAsync(Guid originId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PagedResult<Coffee>());
    }

    public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CoffeeById?.Id == id ? CoffeeById : null);
    }

    public Task<Coffee?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CoffeeByBarcode?.Barcode == barcode ? CoffeeByBarcode : null);
    }

    public Task<Coffee?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CoffeeById?.Id == id ? CoffeeById : null);
    }

    public Task AddAsync(Coffee coffee, CancellationToken cancellationToken = default)
    {
        AddedCoffee = coffee;
        Coffees.Add(coffee);
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

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Coffees.Count);
    }
}
