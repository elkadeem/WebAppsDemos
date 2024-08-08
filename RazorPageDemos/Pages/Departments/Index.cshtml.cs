using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Entities;
using RazorPageDemos.Repositories;
using X.PagedList;

namespace RazorPageDemos.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;
        private readonly IDepartmentRepository _departmentRepository;

        public IndexModel(RazorPageDemos.Model.AdventureworksDbContext context
            , IDepartmentRepository departmentRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string Keyword { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        private const int PageSize = 5;
        public StaticPagedList<DepartmentDto> Departments { get; set; } = default!;

        public async Task OnGetAsync()
        {
            IQueryable<Department> query = _context.Departments;
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                query = query.Where(p => p.Name.Contains(Keyword));
            }

            int pageIndex = CurrentPage - 1;
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            int totalItemsCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.Name)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .Select(c => new DepartmentDto
                {
                    DepartmentId = c.DepartmentId,
                    Name = c.Name,
                    GroupName = c.GroupName,
                    ModifiedDate = c.ModifiedDate
                })
                .ToListAsync();

            Departments = new StaticPagedList<DepartmentDto>(items, pageIndex + 1, PageSize, totalItemsCount);
        }

        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
