using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPageDemos.Entities;
using RazorPageDemos.Model;

namespace RazorPageDemos.Areas.Employees.Pages.Employees
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
        ViewData["BusinessEntityId"] = new SelectList(_context.People, "BusinessEntityId", "BusinessEntityId");
            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
