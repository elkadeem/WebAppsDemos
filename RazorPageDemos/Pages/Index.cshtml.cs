using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RazorPageDemos.Options;

namespace RazorPageDemos.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VacationsRequestsOption _vacationsRequestsOption;
        public IndexModel(
            IOptions<VacationsRequestsOption> vacationsRequestsOption
            , IOptionsSnapshot<VacationsRequestsOption> vacationsRequestsOptionGet
            , IOptionsMonitor<VacationsRequestsOption> vacationsRequestsOptionMonitor
            , ILogger<IndexModel> logger)
        {
            _vacationsRequestsOption = vacationsRequestsOption.Value;
            
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
