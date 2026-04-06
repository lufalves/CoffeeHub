using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Reviews;

[Authorize]
public class CreateModel(
    IReviewService reviewService,
    ICoffeeService coffeeService) : PageModel
{
    [BindProperty]
    public ReviewFormModel Input { get; set; } = new();

    public IReadOnlyList<Coffee> Coffees { get; private set; } = Array.Empty<Coffee>();

    public async Task<IActionResult> OnGetAsync(Guid? coffeeId, CancellationToken cancellationToken)
    {
        await LoadReferenceDataAsync(cancellationToken);

        if (coffeeId.HasValue)
        {
            Input.CoffeeId = coffeeId.Value;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferenceDataAsync(cancellationToken);
            return Page();
        }

        var review = Input.ToEntity(GetCurrentUserId());

        try
        {
            review = await reviewService.CreateAsync(review, cancellationToken);
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

        TempData["StatusMessage"] = "Review created.";
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
