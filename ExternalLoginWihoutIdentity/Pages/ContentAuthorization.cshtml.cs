using ExternalLoginWihoutIdentity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExternalLoginWihoutIdentity.Pages
{
    [Authorize(Policy = "Authenticated")]
    public class ContentAuthorizationModel : PageModel
    {
        private readonly IAuthorizationService _authorizationService;

        public ContentAuthorizationModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public int Age { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {            
            // 21 years old show content 123
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new MinimumAgeRequirement(21));
            if(authorizationResult.Succeeded)
            {
                Age = 21;
                return Page();
            }

            authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new MinimumAgeRequirement(18));
            if(authorizationResult.Succeeded)
            {
                Age = 18;
                return Page();
            }
            // 18 years old show content 12

            return Unauthorized();
        }
    }
}
