using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Roastery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Roasteries;

[Authorize]
public class IndexModel(IRoasteryService roasteryService) : PageModel
{
    public IReadOnlyList<Roastery> Roasteries { get; private set; } = Array.Empty<Roastery>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Roasteries = await roasteryService.GetAllAsync(cancellationToken);
    }
}
