using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageDemos.Entities;
using System.ComponentModel.DataAnnotations;

namespace RazorPageDemos.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(RazorPageDemos.Model.AdventureworksDbContext context
            , ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            _logger.LogInformation("Create page visited by user {UserName}"
                , User.Identity?.Name);
            return Page();
        }

        [BindProperty]
        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; } = default!;

        [BindProperty]
        [Required]
        [MaxLength(50)]
        public string GroupName { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(DepartmentName)
                    && _context.Departments.Any(d => d.Name == DepartmentName))
                {
                    _logger.LogWarning("Department name '{DepartmentName}' already exists"
                        , DepartmentName);
                    ModelState.AddModelError(nameof(DepartmentName), "Department name already exists");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state");
                    return Page();
                }

                _context.Departments.Add(new Department
                {
                    Name = DepartmentName,
                    GroupName = GroupName
                });

                await _context.SaveChangesAsync();
                TempData["Message"] = "Department created successfully";
                _logger.LogInformation("Department '{DepartmentName}' created by user '{UserName}'"
                    , DepartmentName, User.Identity?.Name);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department '{DepartmentName}'"
                    , DepartmentName);
                return Page();
            }
        }
    }


}
