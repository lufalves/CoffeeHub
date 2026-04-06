using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Reviews;

[Authorize]
public class IndexModel(IReviewService reviewService) : PageModel
{
    public IReadOnlyList<Review> Reviews { get; private set; } = Array.Empty<Review>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Reviews = await reviewService.GetByUserIdAsync(userId, cancellationToken);
    }
}
