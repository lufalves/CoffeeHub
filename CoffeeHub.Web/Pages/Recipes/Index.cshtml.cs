using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Recipes;

[Authorize]
public class IndexModel(IRecipeService recipeService) : PageModel
{
    public IReadOnlyList<Recipe> Recipes { get; private set; } = Array.Empty<Recipe>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Recipes = await recipeService.GetByUserIdAsync(userId, cancellationToken);
    }
}
