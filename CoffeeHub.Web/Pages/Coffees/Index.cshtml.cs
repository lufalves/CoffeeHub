using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Coffees;

[Authorize]
public class IndexModel(ICoffeeService coffeeService) : PageModel
{
    private const int PageSize = 20;

    [TempData]
    public string? StatusMessage { get; set; }

    public PagedResult<Coffee> PagedCoffees { get; private set; } = new();

    public async Task OnGetAsync(int page = 1, CancellationToken cancellationToken = default)
    {
        PagedCoffees = await coffeeService.GetPagedAsync(page, PageSize, cancellationToken);
    }
}
