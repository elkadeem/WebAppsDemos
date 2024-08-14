using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Model;

namespace RazorPageDemos.ViewComponents
{
    public class EmployeeDetailsViewComponent : ViewComponent
    {
        private readonly AdventureworksDbContext _dbContext;

        public EmployeeDetailsViewComponent(AdventureworksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.BusinessEntity)
                .FirstOrDefaultAsync(c => c.BusinessEntityId == id);

            return View(employee);
        }
    }
}
