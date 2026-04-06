using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.Web.Controllers;

/// <summary>
/// Handles culture switching via cookie-based persistence.
/// </summary>
[Route("[controller]/[action]")]
public class CultureController : Controller
{
    public IActionResult Set(string culture, string returnUrl)
    {
        if (!string.Equals(culture, "en-US", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(culture, "pt-BR", StringComparison.OrdinalIgnoreCase))
        {
            culture = "en-US";
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) });

        if (Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return LocalRedirect("/");
    }
}
