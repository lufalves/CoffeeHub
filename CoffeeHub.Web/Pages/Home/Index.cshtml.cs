using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Home;

[Authorize]
public class IndexModel(ICoffeeService coffeeService) : PageModel
{
    public string WelcomeName { get; private set; } = "CoffeeHub User";
    public IReadOnlyList<Coffee> Coffees { get; private set; } = Array.Empty<Coffee>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(User.Identity?.Name))
        {
            WelcomeName = User.Identity.Name;
        }

        Coffees = await coffeeService.GetAllAsync(cancellationToken);
    }
}
