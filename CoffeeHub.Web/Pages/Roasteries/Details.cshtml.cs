using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.Roastery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Roasteries;

[Authorize]
public class DetailsModel(
    IRoasteryService roasteryService,
    ICoffeeService coffeeService) : PageModel
{
    private const int PageSize = 12;

    public Roastery? Roastery { get; private set; }
    public PagedResult<Coffee> PagedCoffees { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, int page = 1, CancellationToken cancellationToken = default)
    {
        Roastery = await roasteryService.GetByIdAsync(id, cancellationToken);
        if (Roastery is null)
        {
            return NotFound();
        }

        PagedCoffees = await coffeeService.GetByRoasteryIdAsync(id, page, PageSize, cancellationToken);
        return Page();
    }
}
