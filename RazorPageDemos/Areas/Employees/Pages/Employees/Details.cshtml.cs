using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Entities;
using RazorPageDemos.Model;

namespace RazorPageDemos.Areas.Employees.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public DetailsModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.BusinessEntityId == id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                Employee = employee;
            }
            return Page();
        }
    }
}
