using System.ComponentModel.DataAnnotations;
using CoffeeHub.Domain.Recipe;

namespace CoffeeHub.Web.Pages.Recipes;

public class RecipeFormModel
{
    [Required(ErrorMessage = "ValidationRequired")]
    [Display(Name = "Coffee")]
    public Guid? CoffeeId { get; set; }

    [Required(ErrorMessage = "ValidationRequired")]
    [Display(Name = "RecipesBrewingMethod")]
    public Guid? BrewingMethodId { get; set; }

    [Required(ErrorMessage = "ValidationRequired")]
    [StringLength(200, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Range(0.01, 2000, ErrorMessage = "ValidationRange")]
    [Display(Name = "RecipesCoffeeAmountLabel")]
    public decimal? CoffeeAmountInGrams { get; set; }

    [Range(0.01, 5000, ErrorMessage = "ValidationRange")]
    [Display(Name = "RecipesWaterAmountLabel")]
    public decimal? WaterAmountInMilliliters { get; set; }

    [Range(0, 36000, ErrorMessage = "ValidationRange")]
    [Display(Name = "RecipesBrewTimeLabel")]
    public int? BrewTimeInSeconds { get; set; }

    [StringLength(4000, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "RecipesInstructions")]
    public string? Instructions { get; set; }

    public void LoadFrom(Recipe recipe)
    {
        CoffeeId = recipe.CoffeeId;
        BrewingMethodId = recipe.BrewingMethodId;
        Title = recipe.Title;
        Description = recipe.Description;
        CoffeeAmountInGrams = recipe.CoffeeAmountInGrams;
        WaterAmountInMilliliters = recipe.WaterAmountInMilliliters;
        BrewTimeInSeconds = recipe.BrewTimeInSeconds;
        Instructions = recipe.Instructions;
    }

    public Recipe ToEntity(Guid userId, Guid? recipeId = null)
    {
        return new Recipe
        {
            Id = recipeId ?? Guid.Empty,
            UserId = userId,
            CoffeeId = CoffeeId ?? Guid.Empty,
            BrewingMethodId = BrewingMethodId ?? Guid.Empty,
            Title = Title.Trim(),
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim(),
            CoffeeAmountInGrams = CoffeeAmountInGrams,
            WaterAmountInMilliliters = WaterAmountInMilliliters,
            BrewTimeInSeconds = BrewTimeInSeconds,
            Instructions = string.IsNullOrWhiteSpace(Instructions) ? null : Instructions.Trim()
        };
    }
}
