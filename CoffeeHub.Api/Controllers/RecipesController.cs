using CoffeeHub.Api.Contracts.Requests;
using CoffeeHub.Api.Contracts.Responses;
using CoffeeHub.Api.Mapping;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class RecipesController(IRecipeService recipeService) : ControllerBase
{
    /// <summary>
    /// Gets all recipes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RecipeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RecipeResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var recipes = await recipeService.GetByUserIdAsync(Guid.Empty, cancellationToken);
        return Ok(recipes.Select(item => item.ToResponse()).ToList());
    }

    /// <summary>
    /// Gets a recipe by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await recipeService.GetByIdAsync(id, cancellationToken);

        if (recipe is null)
        {
            return NotFound();
        }

        return Ok(recipe.ToResponse());
    }

    /// <summary>
    /// Gets all recipes for a coffee.
    /// </summary>
    [HttpGet("coffee/{coffeeId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<RecipeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RecipeResponse>>> GetByCoffee(Guid coffeeId, CancellationToken cancellationToken)
    {
        var recipes = await recipeService.GetByCoffeeIdAsync(coffeeId, cancellationToken);
        return Ok(recipes.Select(item => item.ToResponse()).ToList());
    }

    /// <summary>
    /// Creates a recipe.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<RecipeResponse>> Create(CreateRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = new Recipe
        {
            UserId = request.UserId,
            CoffeeId = request.CoffeeId,
            BrewingMethodId = request.BrewingMethodId,
            Title = request.Title,
            Description = request.Description,
            CoffeeAmountInGrams = request.CoffeeAmountInGrams,
            WaterAmountInMilliliters = request.WaterAmountInMilliliters,
            BrewTimeInSeconds = request.BrewTimeInSeconds,
            Instructions = request.Instructions
        };

        var createdRecipe = await recipeService.CreateAsync(recipe, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdRecipe.Id }, createdRecipe.ToResponse());
    }

    /// <summary>
    /// Updates a recipe.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecipeResponse>> Update(Guid id, UpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var updatedRecipe = await recipeService.UpdateAsync(
            new Recipe
            {
                Id = id,
                UserId = request.UserId,
                CoffeeId = request.CoffeeId,
                BrewingMethodId = request.BrewingMethodId,
                Title = request.Title,
                Description = request.Description,
                CoffeeAmountInGrams = request.CoffeeAmountInGrams,
                WaterAmountInMilliliters = request.WaterAmountInMilliliters,
                BrewTimeInSeconds = request.BrewTimeInSeconds,
                Instructions = request.Instructions
            },
            cancellationToken);

        if (updatedRecipe is null)
        {
            return NotFound();
        }

        return Ok(updatedRecipe.ToResponse());
    }

    /// <summary>
    /// Soft-deletes a recipe.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await recipeService.SoftDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
