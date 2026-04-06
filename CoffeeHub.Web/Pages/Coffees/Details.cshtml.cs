using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Coffees;

[Authorize]
public class DetailsModel(ICoffeeService coffeeService) : PageModel
{
    [TempData]
    public string? StatusMessage { get; set; }

    public Coffee? Coffee { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        Coffee = await coffeeService.GetByIdWithDetailsAsync(id, cancellationToken);

        return Coffee is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await coffeeService.SoftDeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Coffee removed from the catalog.";
        return RedirectToPage("/Coffees/Index");
    }
}
