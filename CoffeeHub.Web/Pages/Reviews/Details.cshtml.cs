using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Reviews;

[Authorize]
public class DetailsModel(IReviewService reviewService) : PageModel
{
    [TempData]
    public string? StatusMessage { get; set; }

    public Review? Review { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        var review = await reviewService.GetByIdAsync(id, cancellationToken);
        if (review is null)
        {
            return NotFound();
        }

        if (review.UserId != GetCurrentUserId())
        {
            return Forbid();
        }

        Review = review;
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var review = await reviewService.GetByIdAsync(id, cancellationToken);
        if (review is null)
        {
            return NotFound();
        }

        if (review.UserId != GetCurrentUserId())
        {
            return Forbid();
        }

        var deleted = await reviewService.SoftDeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Review removed.";
        return RedirectToPage("/Reviews/Index");
    }

    private Guid GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(rawUserId, out var userId)
            ? userId
            : throw new InvalidOperationException("Authenticated user identifier is invalid.");
    }
}
