using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Recipes;

[Authorize]
public class EditModel(
    IRecipeService recipeService,
    ICoffeeService coffeeService,
    IBrewingMethodService brewingMethodService) : PageModel
{
    [BindProperty]
    public RecipeFormModel Input { get; set; } = new();

    public IReadOnlyList<Coffee> Coffees { get; private set; } = Array.Empty<Coffee>();
    public IReadOnlyList<BrewingMethod> BrewingMethods { get; private set; } = Array.Empty<BrewingMethod>();

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

        Input.LoadFrom(recipe);
        await LoadReferenceDataAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        var recipe = Input.ToEntity(GetCurrentUserId(), id);

        try
        {
            recipe = await recipeService.UpdateAsync(recipe, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        if (recipe is null)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Recipe updated.";
        return RedirectToPage("/Recipes/Details", new { id = recipe.Id });
    }

    private async Task LoadReferenceDataAsync(CancellationToken cancellationToken)
    {
        Coffees = await coffeeService.GetAllAsync(cancellationToken);
        BrewingMethods = await brewingMethodService.GetAllOrderedAsync(cancellationToken);
    }

    private Guid GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(rawUserId, out var userId)
            ? userId
            : throw new InvalidOperationException("Authenticated user identifier is invalid.");
    }
}
