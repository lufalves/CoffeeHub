using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using CoffeeHub.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeHub.Web.Pages.Profile;

[Authorize]
public class IndexModel(IUserService userService) : PageModel
{
    [BindProperty]
    public ProfileInputModel ProfileInput { get; set; } = new();

    [BindProperty]
    public AvatarInputModel AvatarInput { get; set; } = new();

    [BindProperty]
    public PasswordInputModel PasswordInput { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public User? CurrentUser { get; private set; }
    public string ActiveSection { get; private set; } = ProfileSections.Profile;

    public async Task<IActionResult> OnGetAsync(string? section, CancellationToken cancellationToken)
    {
        ActiveSection = NormalizeSection(section);

        var user = await LoadCurrentUserAsync(cancellationToken);

        if (user is null)
        {
            return await HandleMissingUserAsync();
        }

        CurrentUser = user;
        PopulateInputs(user);

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateProfileAsync(CancellationToken cancellationToken)
    {
        ActiveSection = ProfileSections.Edit;
        ClearModelStateFor(nameof(AvatarInput), nameof(PasswordInput));

        if (!ModelState.IsValid)
        {
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        User? updatedUser;

        try
        {
            updatedUser = await userService.UpdateProfileAsync(GetCurrentUserId(), ProfileInput.Name, ProfileInput.Email, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        if (updatedUser is null)
        {
            return await HandleMissingUserAsync();
        }

        await RefreshAuthenticationAsync(updatedUser);

        StatusMessage = "Profile details updated.";
        return RedirectToPage(new { section = ProfileSections.Profile });
    }

    public async Task<IActionResult> OnPostUpdateAvatarAsync(CancellationToken cancellationToken)
    {
        ActiveSection = ProfileSections.Photo;
        ClearModelStateFor(nameof(ProfileInput), nameof(PasswordInput));

        if (!ModelState.IsValid)
        {
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        User? updatedUser;

        try
        {
            updatedUser = await userService.UpdateAvatarAsync(GetCurrentUserId(), AvatarInput.AvatarUrl, cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        if (updatedUser is null)
        {
            return await HandleMissingUserAsync();
        }

        await RefreshAuthenticationAsync(updatedUser);

        StatusMessage = "Profile photo updated.";
        return RedirectToPage(new { section = ProfileSections.Profile });
    }

    public async Task<IActionResult> OnPostChangePasswordAsync(CancellationToken cancellationToken)
    {
        ActiveSection = ProfileSections.Password;
        ClearModelStateFor(nameof(ProfileInput), nameof(AvatarInput));

        if (!ModelState.IsValid)
        {
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        bool passwordChanged;

        try
        {
            passwordChanged = await userService.ChangePasswordAsync(
                GetCurrentUserId(),
                PasswordInput.CurrentPassword,
                PasswordInput.NewPassword,
                cancellationToken);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return await ReloadCurrentUserPageAsync(cancellationToken);
        }

        if (!passwordChanged)
        {
            return await HandleMissingUserAsync();
        }

        StatusMessage = "Password updated.";
        return RedirectToPage(new { section = ProfileSections.Profile });
    }

    public string GetSectionClass(string section)
    {
        return ActiveSection == section ? "is-active" : string.Empty;
    }

    private async Task<IActionResult> ReloadCurrentUserPageAsync(CancellationToken cancellationToken)
    {
        var user = await LoadCurrentUserAsync(cancellationToken);

        if (user is null)
        {
            return await HandleMissingUserAsync();
        }

        CurrentUser = user;
        MergeInputs(user);

        return Page();
    }

    private async Task<User?> LoadCurrentUserAsync(CancellationToken cancellationToken)
    {
        return await userService.GetByIdAsync(GetCurrentUserId(), cancellationToken);
    }

    private void PopulateInputs(User user)
    {
        ProfileInput = new ProfileInputModel
        {
            Name = user.Name,
            Email = user.Email
        };

        AvatarInput = new AvatarInputModel
        {
            AvatarUrl = user.AvatarUrl
        };

        PasswordInput = new PasswordInputModel();
    }

    private void MergeInputs(User user)
    {
        ProfileInput.Name = string.IsNullOrWhiteSpace(ProfileInput.Name) ? user.Name : ProfileInput.Name;
        ProfileInput.Email = string.IsNullOrWhiteSpace(ProfileInput.Email) ? user.Email : ProfileInput.Email;
        AvatarInput.AvatarUrl ??= user.AvatarUrl;
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new InvalidOperationException("Authenticated user identifier is invalid.");
    }

    private async Task<IActionResult> HandleMissingUserAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Account/Login");
    }

    private async Task RefreshAuthenticationAsync(User user)
    {
        var authentication = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var isPersistent = authentication.Properties?.IsPersistent ?? false;

        await AuthenticationCookieHelper.SignInUserAsync(HttpContext, user, isPersistent);
    }

    private static string NormalizeSection(string? section)
    {
        return section?.Trim().ToLowerInvariant() switch
        {
            ProfileSections.Edit => ProfileSections.Edit,
            ProfileSections.Photo => ProfileSections.Photo,
            ProfileSections.Password => ProfileSections.Password,
            ProfileSections.Recipes => ProfileSections.Recipes,
            ProfileSections.Reviews => ProfileSections.Reviews,
            _ => ProfileSections.Profile
        };
    }

    private void ClearModelStateFor(params string[] prefixes)
    {
        foreach (var key in ModelState.Keys.Where(key => prefixes.Any(prefix => key.StartsWith(prefix, StringComparison.Ordinal))).ToList())
        {
            ModelState.Remove(key);
        }
    }

    public static class ProfileSections
    {
        public const string Profile = "profile";
        public const string Edit = "edit";
        public const string Photo = "photo";
        public const string Password = "password";
        public const string Recipes = "recipes";
        public const string Reviews = "reviews";
    }

    public class ProfileInputModel
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string Email { get; set; } = string.Empty;
    }

    public class AvatarInputModel
    {
        [Url]
        [StringLength(500)]
        public string? AvatarUrl { get; set; }
    }

    public class PasswordInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
