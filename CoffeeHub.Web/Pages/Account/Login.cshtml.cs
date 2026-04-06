using System.ComponentModel.DataAnnotations;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using CoffeeHub.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Account;

public class LoginModel(IAuthService authService, LoginAttemptTracker loginTracker) : PageModel
{
    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Home/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (loginTracker.IsLockedOut(Input.Email))
        {
            var remaining = loginTracker.GetLockoutRemaining(Input.Email);
            ModelState.AddModelError(string.Empty, $"Too many failed attempts. Try again in {remaining?.Minutes:N0} minutes.");
            return Page();
        }

        User? user;

        try
        {
            user = await authService.ValidateCredentialsAsync(Input.Email, Input.Password, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Page();
        }

        if (user is null)
        {
            loginTracker.RecordFailure(Input.Email);
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }

        loginTracker.RecordSuccess(Input.Email);
        await AuthenticationCookieHelper.SignInUserAsync(HttpContext, user, Input.RememberMe, user.Role);

        return RedirectToPage("/Home/Index");
    }

    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    // Handler for extending session via AJAX
    [IgnoreAntiforgeryToken] // For AJAX calls, we'll handle validation differently
    public IActionResult OnPostExtendSession()
    {
        // This endpoint just needs to be hit to reset the session sliding expiration
        // The cookie middleware will automatically reset the expiration on any authenticated request
        return new JsonResult(new { success = true });
    }
}
