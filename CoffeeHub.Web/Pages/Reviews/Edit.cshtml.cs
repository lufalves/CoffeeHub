using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Reviews;

[Authorize]
public class EditModel(
    IReviewService reviewService,
    ICoffeeService coffeeService) : PageModel
{
    [BindProperty]
    public ReviewFormModel Input { get; set; } = new();

    public IReadOnlyList<Coffee> Coffees { get; private set; } = Array.Empty<Coffee>();

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

        Input.LoadFrom(review);
        await LoadReferenceDataAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        var review = Input.ToEntity(GetCurrentUserId(), id);

        try
        {
            review = await reviewService.UpdateAsync(review, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        if (review is null)
        {
            return NotFound();
        }

        TempData["StatusMessage"] = "Review updated.";
        return RedirectToPage("/Reviews/Details", new { id = review.Id });
    }

    private async Task LoadReferenceDataAsync(CancellationToken cancellationToken)
    {
        Coffees = await coffeeService.GetAllAsync(cancellationToken);
    }

    private Guid GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(rawUserId, out var userId)
            ? userId
            : throw new InvalidOperationException("Authenticated user identifier is invalid.");
    }
}
