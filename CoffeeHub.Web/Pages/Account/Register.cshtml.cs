using System.ComponentModel.DataAnnotations;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using CoffeeHub.Web.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Account;

public class RegisterModel(IAuthService authService, IUserService userService) : PageModel
{
    [BindProperty]
    public RegisterInputModel Input { get; set; } = new();

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

        User user;

        try
        {
            user = await authService.RegisterAsync(Input.Name, Input.Email, Input.Password, cancellationToken: cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Page();
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return Page();
        }

        var totalUsers = await userService.GetTotalCountAsync(cancellationToken);
        var role = totalUsers <= 1 ? "Admin" : "User";
        await AuthenticationCookieHelper.SignInUserAsync(HttpContext, user, true, role);

        return RedirectToPage("/Home/Index");
    }

    public class RegisterInputModel
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
