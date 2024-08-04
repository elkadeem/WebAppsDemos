using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorPageDemos.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public DeleteModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DepartmentDto Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {                
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            } else
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
