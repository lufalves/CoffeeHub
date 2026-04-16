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
        [Required(ErrorMessage = "ValidationRequired")]
        [StringLength(150, ErrorMessage = "ValidationMaxLength")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "ValidationRequired")]
        [EmailAddress(ErrorMessage = "ValidationEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "ValidationRequired")]
        [MinLength(6, ErrorMessage = "ValidationMinLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "ValidationRequired")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "ValidationCompare")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
