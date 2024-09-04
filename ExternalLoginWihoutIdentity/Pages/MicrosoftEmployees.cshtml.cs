using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalLoginWihoutIdentity.Pages
{
    [Authorize(Policy = "MicrosoftEmployee")]
    public class MicrosoftEmployeesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
