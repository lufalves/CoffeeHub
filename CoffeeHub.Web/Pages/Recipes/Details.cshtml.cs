using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Recipes;

[Authorize]
public class DetailsModel(IRecipeService recipeService) : PageModel
{
    [TempData]
    public string? StatusMessage { get; set; }

    public Recipe? Recipe { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await recipeService.GetByIdAsync(id, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        if (recipe.UserId != GetCurrentUserId())
        {
            return Forbid();
        }

        Recipe = recipe;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await recipeService.GetByIdAsync(id, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        if (recipe.UserId != GetCurrentUserId())
        {
            return Forbid();
        }

        var deleted = await recipeService.SoftDeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Recipe removed.";
        return RedirectToPage("/Recipes/Index");
    }

    private Guid GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(rawUserId, out var userId)
            ? userId
            : throw new InvalidOperationException("Authenticated user identifier is invalid.");
    }
}
