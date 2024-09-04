using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalLoginWihoutIdentity.Pages
{
    [Authorize(Policy = "GoogleEmployee")]
    public class GoogleEmployeesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
