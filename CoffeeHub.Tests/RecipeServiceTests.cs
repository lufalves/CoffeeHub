using CoffeeHub.Application.Services;
using CoffeeHub.Tests.Builders;
using CoffeeHub.Tests.Common;
using CoffeeHub.Tests.Fakes;

namespace CoffeeHub.Tests;

public class RecipeServiceTests : ServiceTestBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateRecipe_WhenDataIsValid()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().Build();

        var created = await service.CreateAsync(recipe, TestContext.Current.CancellationToken);

        Assert.Same(recipe, created);
        Assert.Single(repository.Recipes);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenUserIdIsEmpty()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithUserId(Guid.Empty).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCoffeeIdIsEmpty()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithCoffeeId(Guid.Empty).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenBrewingMethodIdIsEmpty()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithBrewingMethodId(Guid.Empty).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_ShouldThrow_WhenTitleIsNullOrEmpty(string? title)
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithTitle(title!).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenTitleExceeds200Chars()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithTitle(new string('T', 201)).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task CreateAsync_ShouldThrow_WhenCoffeeAmountIsNegativeOrZero(decimal amount)
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithCoffeeAmount(amount).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-500)]
    public async Task CreateAsync_ShouldThrow_WhenWaterAmountIsNegativeOrZero(decimal amount)
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithWaterAmount(amount).Build();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(recipe, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenRecipeDoesNotExist()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var recipe = new TestRecipeBuilder().WithId(Guid.NewGuid()).Build();

        var result = await service.UpdateAsync(recipe, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRecipe_WhenRecipeExists()
    {
        var existing = new TestRecipeBuilder().WithId(Guid.NewGuid()).WithTitle("Old Title").Build();

        var repository = new FakeRecipeRepository();
        repository.Recipes.Add(existing);

        var service = new RecipeService(repository, Logger<RecipeService>());

        var update = new TestRecipeBuilder()
            .WithId(existing.Id)
            .WithTitle("New Title")
            .Build();

        var result = await service.UpdateAsync(update, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("New Title", result.Title);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldReturnFalse_WhenRecipeDoesNotExist()
    {
        var repository = new FakeRecipeRepository();
        var service = new RecipeService(repository, Logger<RecipeService>());

        var result = await service.SoftDeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.False(result);
        Assert.Null(repository.SoftDeletedRecipeId);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldDelete_WhenRecipeExists()
    {
        var existing = new TestRecipeBuilder().WithId(Guid.NewGuid()).Build();

        var repository = new FakeRecipeRepository();
        repository.Recipes.Add(existing);

        var service = new RecipeService(repository, Logger<RecipeService>());

        var result = await service.SoftDeleteAsync(existing.Id, TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Equal(existing.Id, repository.SoftDeletedRecipeId);
    }
}
