using CoffeeHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Coffees;

[Authorize]
public class CreateModel(ICoffeeService coffeeService) : PageModel
{
    [BindProperty]
    public CoffeeFormModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!Input.TryBuild(ModelState, out var coffee))
        {
            return Page();
        }

        try
        {
            coffee = await coffeeService.CreateAsync(coffee, cancellationToken);
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

        TempData["StatusMessage"] = "Coffee registered.";
        return RedirectToPage("/Coffees/Details", new { id = coffee.Id });
    }
}
