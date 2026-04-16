using CoffeeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Localization;

namespace CoffeeHub.Web.Pages.Coffees;

[Authorize]
public class EditModel(ICoffeeService coffeeService, IStringLocalizer<SharedResources> localizer) : PageModel
{
    [BindProperty]
    public CoffeeFormModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        var coffee = await coffeeService.GetByIdAsync(id, cancellationToken);

        if (coffee is null)
        {
            return NotFound();
        }

        Input.LoadFrom(coffee);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!Input.TryBuild(ModelState, localizer, out var coffee, id))
        {
            return Page();
        }

        try
        {
            coffee = await coffeeService.UpdateAsync(coffee, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Page();
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Page();
        }

        if (coffee is null)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Coffee updated.";
        return RedirectToPage("/Coffees/Details", new { id = coffee.Id });
    }
}
