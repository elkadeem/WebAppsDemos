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
    public class IndexModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public IndexModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees
                .Include(e => e.BusinessEntity).ToListAsync();
        }
    }
}
