using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalLoginWihoutIdentity.Pages
{
    [Authorize(Policy = "AllowedFamilies")]
    public class AllowedFamiliesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
