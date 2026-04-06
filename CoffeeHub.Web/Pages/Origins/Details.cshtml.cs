using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.Origin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Origins;

[Authorize]
public class DetailsModel(
    IOriginService originService,
    ICoffeeService coffeeService) : PageModel
{
    private const int PageSize = 12;

    public Origin? Origin { get; private set; }
    public PagedResult<Coffee> PagedCoffees { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, int page = 1, CancellationToken cancellationToken = default)
    {
        Origin = await originService.GetByIdAsync(id, cancellationToken);
        if (Origin is null)
        {
            return NotFound();
        }

        PagedCoffees = await coffeeService.GetByOriginIdAsync(id, page, PageSize, cancellationToken);
        return Page();
    }
}
