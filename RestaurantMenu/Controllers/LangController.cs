using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace RestaurantMenu.Controllers
{
    public class LangController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("SwitchLanguage", new { lang = "ar-Jo" });
        }

        public IActionResult SwitchLanguage(string cluture, string returnUrl)
        {
            if (string.IsNullOrEmpty(cluture))
                cluture = "en-US"; // Default to English if no culture is provided.

            // Set culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cluture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1), IsEssential = true }
            );

            // Store in session (optional)
            HttpContext.Session.SetString("clutureLanguage", cluture);

            // **Redirect to the referring page** (or fallback)
            return LocalRedirect(returnUrl ?? "/");
        }
    }
}
