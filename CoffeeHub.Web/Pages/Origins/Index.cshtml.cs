using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Origin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Origins;

[Authorize]
public class IndexModel(IOriginService originService) : PageModel
{
    public IReadOnlyList<Origin> Origins { get; private set; } = Array.Empty<Origin>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Origins = await originService.GetAllAsync(cancellationToken);
    }
}
