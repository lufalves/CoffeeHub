using CoffeeHub.Domain.BeanVariety;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.Farm;
using CoffeeHub.Domain.Origin;
using CoffeeHub.Domain.Recipe;
using CoffeeHub.Domain.Review;
using CoffeeHub.Domain.RoastLevel;
using CoffeeHub.Domain.Roastery;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Tests.Builders;

public sealed class TestCoffeeBuilder
{
    private readonly Coffee _coffee = new()
    {
        Barcode = "7891000100103",
        Name = "Test Coffee",
        Description = "Test description",
        RoasteryId = Guid.NewGuid(),
        OriginId = Guid.NewGuid(),
        FarmId = Guid.NewGuid(),
        BeanVarietyId = Guid.NewGuid(),
        RoastLevelId = Guid.NewGuid()
    };

    public TestCoffeeBuilder WithId(Guid id)
    {
        _coffee.Id = id;
        return this;
    }

    public TestCoffeeBuilder WithBarcode(string barcode)
    {
        _coffee.Barcode = barcode;
        return this;
    }

    public TestCoffeeBuilder WithName(string name)
    {
        _coffee.Name = name;
        return this;
    }

    public TestCoffeeBuilder WithRoasteryId(Guid roasteryId)
    {
        _coffee.RoasteryId = roasteryId;
        return this;
    }

    public Coffee Build() => _coffee;
}

public sealed class TestUserBuilder
{
    private readonly User _user = new()
    {
        Name = "Test User",
        Email = "test@coffeehub.dev",
        PasswordHash = "hashed:Password123!",
        Role = "User"
    };

    public TestUserBuilder WithId(Guid id)
    {
        _user.Id = id;
        return this;
    }

    public TestUserBuilder WithName(string name)
    {
        _user.Name = name;
        return this;
    }

    public TestUserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public TestUserBuilder WithPasswordHash(string passwordHash)
    {
        _user.PasswordHash = passwordHash;
        return this;
    }

    public TestUserBuilder WithRole(string role)
    {
        _user.Role = role;
        return this;
    }

    public User Build() => _user;
}

public sealed class TestRecipeBuilder
{
    private readonly Recipe _recipe = new()
    {
        UserId = Guid.NewGuid(),
        CoffeeId = Guid.NewGuid(),
        BrewingMethodId = Guid.NewGuid(),
        Title = "Test Recipe",
        Description = "Recipe description",
        CoffeeAmountInGrams = 20,
        WaterAmountInMilliliters = 320,
        BrewTimeInSeconds = 180,
        Instructions = "Brew slowly"
    };

    public TestRecipeBuilder WithId(Guid id)
    {
        _recipe.Id = id;
        return this;
    }

    public TestRecipeBuilder WithUserId(Guid userId)
    {
        _recipe.UserId = userId;
        return this;
    }

    public TestRecipeBuilder WithCoffeeId(Guid coffeeId)
    {
        _recipe.CoffeeId = coffeeId;
        return this;
    }

    public TestRecipeBuilder WithTitle(string title)
    {
        _recipe.Title = title;
        return this;
    }

    public TestRecipeBuilder WithCoffeeAmount(decimal? amount)
    {
        _recipe.CoffeeAmountInGrams = amount;
        return this;
    }

    public TestRecipeBuilder WithWaterAmount(decimal? amount)
    {
        _recipe.WaterAmountInMilliliters = amount;
        return this;
    }

    public TestRecipeBuilder WithBrewingMethodId(Guid brewingMethodId)
    {
        _recipe.BrewingMethodId = brewingMethodId;
        return this;
    }

    public Recipe Build() => _recipe;
}

public sealed class TestReviewBuilder
{
    private readonly Review _review = new()
    {
        UserId = Guid.NewGuid(),
        CoffeeId = Guid.NewGuid(),
        Rating = 4,
        Comment = "Great coffee"
    };

    public TestReviewBuilder WithId(Guid id)
    {
        _review.Id = id;
        return this;
    }

    public TestReviewBuilder WithUserId(Guid userId)
    {
        _review.UserId = userId;
        return this;
    }

    public TestReviewBuilder WithCoffeeId(Guid coffeeId)
    {
        _review.CoffeeId = coffeeId;
        return this;
    }

    public TestReviewBuilder WithRating(decimal rating)
    {
        _review.Rating = rating;
        return this;
    }

    public TestReviewBuilder WithComment(string? comment)
    {
        _review.Comment = comment;
        return this;
    }

    public Review Build() => _review;
}

public static class IntegrationEntityFactory
{
    public static Roastery CreateRoastery(string name = "Roastery") => new() { Name = name };

    public static Origin CreateOrigin(string country = "Brazil") => new() { Country = country };

    public static Farm CreateFarm(Guid? originId = null, string name = "Farm") => new() { Name = name, OriginId = originId };

    public static BeanVariety CreateBeanVariety(string name = "Bourbon") => new() { Name = name };

    public static RoastLevel CreateRoastLevel(string name = "Medium", int displayOrder = 1) => new() { Name = name, DisplayOrder = displayOrder };

    public static BrewingMethod CreateBrewingMethod(string name = "V60") => new() { Name = name };
}
