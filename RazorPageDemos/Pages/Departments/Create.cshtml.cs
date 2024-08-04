using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageDemos.Entities;
using System.ComponentModel.DataAnnotations;

namespace RazorPageDemos.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public CreateModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
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
            if(!string.IsNullOrWhiteSpace(DepartmentName)
                && _context.Departments.Any(d => d.Name == DepartmentName))
            {
                ModelState.AddModelError(nameof(DepartmentName), "Department name already exists");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Departments.Add(new Department
            {
                Name = DepartmentName,
                GroupName = GroupName
            });
            await _context.SaveChangesAsync();
            TempData["Message"] = "Department created successfully";
            return RedirectToPage("./Index");
        }
    }

   
}
