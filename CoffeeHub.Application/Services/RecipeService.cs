using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Recipe;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class RecipeService(IRecipeRepository recipeRepository, ILogger<RecipeService> _logger) : IRecipeService
{
    public Task<IReadOnlyList<Recipe>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(userId, nameof(userId), "User id");
        return recipeRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public Task<IReadOnlyList<Recipe>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(coffeeId, nameof(coffeeId), "Coffee id");
        return recipeRepository.GetByCoffeeIdAsync(coffeeId, cancellationToken);
    }

    public Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(id, nameof(id), "Recipe id");
        return recipeRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Recipe> CreateAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        EntityValidator.ThrowIfEmptyGuid(recipe.UserId, nameof(recipe), "User id");
        EntityValidator.ThrowIfEmptyGuid(recipe.CoffeeId, nameof(recipe), "Coffee id");
        EntityValidator.ThrowIfEmptyGuid(recipe.BrewingMethodId, nameof(recipe), "Brewing method id");
        EntityValidator.ThrowIfNullOrWhiteSpace(recipe.Title, nameof(recipe), "Recipe title");
        EntityValidator.ThrowIfExceedsLength(recipe.Title, 200, nameof(recipe), "Recipe title");

        if (recipe.CoffeeAmountInGrams.HasValue && recipe.CoffeeAmountInGrams.Value <= 0)
            throw new ArgumentException("Coffee amount must be positive.", nameof(recipe));

        if (recipe.WaterAmountInMilliliters.HasValue && recipe.WaterAmountInMilliliters.Value <= 0)
            throw new ArgumentException("Water amount must be positive.", nameof(recipe));

        await recipeRepository.AddAsync(recipe, cancellationToken);
        _logger.LogInformation("Recipe created: {RecipeId} - {Title}", recipe.Id, recipe.Title);
        return recipe;
    }

    public async Task<Recipe?> UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        if (recipe.Id == Guid.Empty)
            throw new ArgumentException("Recipe id must be informed.", nameof(recipe));

        ArgumentNullException.ThrowIfNull(recipe);

        var existingRecipe = await recipeRepository.GetByIdAsync(recipe.Id, cancellationToken);
        if (existingRecipe is null)
        {
            _logger.LogWarning("Attempted to update non-existent recipe: {RecipeId}", recipe.Id);
            return null;
        }

        existingRecipe.Title = EntityValidator.NormalizeName(recipe.Title);
        existingRecipe.Description = EntityValidator.NormalizeOptionalString(recipe.Description);
        existingRecipe.CoffeeAmountInGrams = recipe.CoffeeAmountInGrams;
        existingRecipe.WaterAmountInMilliliters = recipe.WaterAmountInMilliliters;
        existingRecipe.BrewTimeInSeconds = recipe.BrewTimeInSeconds;
        existingRecipe.Instructions = EntityValidator.NormalizeOptionalString(recipe.Instructions);

        await recipeRepository.UpdateAsync(existingRecipe, cancellationToken);
        _logger.LogInformation("Recipe updated: {RecipeId}", recipe.Id);
        return existingRecipe;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Recipe id must be informed.", nameof(id));

        var existingRecipe = await recipeRepository.GetByIdAsync(id, cancellationToken);
        if (existingRecipe is null)
        {
            _logger.LogWarning("Attempted to delete non-existent recipe: {RecipeId}", id);
            return false;
        }

        await recipeRepository.SoftDeleteAsync(id, cancellationToken);
        _logger.LogInformation("Recipe soft-deleted: {RecipeId}", id);
        return true;
    }
}
