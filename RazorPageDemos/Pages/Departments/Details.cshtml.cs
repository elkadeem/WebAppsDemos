using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Entities;

namespace RazorPageDemos.Pages.Departments
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public DetailsModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        public DepartmentDto Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null && id <= 0)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Select(c => new DepartmentDto
                {
                    DepartmentId = c.DepartmentId,
                    Name = c.Name,
                    GroupName = c.GroupName,
                    ModifiedDate = c.ModifiedDate
                })
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }
    }
}
