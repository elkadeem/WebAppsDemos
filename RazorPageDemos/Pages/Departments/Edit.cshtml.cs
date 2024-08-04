using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Entities;
using System.ComponentModel.DataAnnotations;

namespace RazorPageDemos.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public EditModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public short DepartmentId { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; } = default!;

        [BindProperty]
        [Required]
        [MaxLength(50)]
        public string GroupName { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            DepartmentId = department.DepartmentId;
            DepartmentName = department.Name;
            GroupName = department.GroupName;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(DepartmentName)
                && _context.Departments.Any(d => d.DepartmentId != DepartmentId 
                && d.Name == DepartmentName))
            {
                ModelState.AddModelError(nameof(DepartmentName), "Department name already exists");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var deparment = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentId == DepartmentId);
            if (deparment == null)
            {
                return NotFound();
            }

            deparment.Name = DepartmentName;
            deparment.GroupName = GroupName;
            deparment.ModifiedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(DepartmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["Message"] = "Department updated successfully";
            return RedirectToPage("./Index");
        }

        private bool DepartmentExists(short id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
