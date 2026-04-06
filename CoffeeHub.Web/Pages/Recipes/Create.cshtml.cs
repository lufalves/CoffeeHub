using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Recipes;

[Authorize]
public class CreateModel(
    IRecipeService recipeService,
    ICoffeeService coffeeService,
    IBrewingMethodService brewingMethodService) : PageModel
{
    [BindProperty]
    public RecipeFormModel Input { get; set; } = new();

    public IReadOnlyList<Coffee> Coffees { get; private set; } = Array.Empty<Coffee>();
    public IReadOnlyList<BrewingMethod> BrewingMethods { get; private set; } = Array.Empty<BrewingMethod>();

    public async Task<IActionResult> OnGetAsync(Guid? coffeeId, CancellationToken cancellationToken)
    {
        await LoadReferenceDataAsync(cancellationToken);

        if (coffeeId.HasValue)
        {
            Input.CoffeeId = coffeeId.Value;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        var recipe = Input.ToEntity(GetCurrentUserId());

        try
        {
            recipe = await recipeService.CreateAsync(recipe, cancellationToken);
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

        TempData["StatusMessage"] = "Recipe created.";
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
