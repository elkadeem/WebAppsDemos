using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SimpleCookieAuth.Pages
{
    public class Login2Model : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string returnUrl)
        {
            // Add authentication logic
            // Create Identity object with name user1 and department It
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, "user1"),
                new Claim(ClaimTypes.Role, "It"),
                new Claim("UserAgent", Request.Headers.UserAgent.ToString())
            }, Program.SecondCookieSchema);

            // Create principal object
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            return SignIn(principal,
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                    IsPersistent = false,
                }
                , CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
