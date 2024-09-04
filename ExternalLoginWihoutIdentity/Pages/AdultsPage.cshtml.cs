using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalLoginWihoutIdentity.Pages
{
    [Authorize(Policy = "MinimumAge18")]
    public class AdultsPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
