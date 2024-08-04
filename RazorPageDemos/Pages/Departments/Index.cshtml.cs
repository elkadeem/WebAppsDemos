using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Entities;
using X.PagedList;

namespace RazorPageDemos.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly RazorPageDemos.Model.AdventureworksDbContext _context;

        public IndexModel(RazorPageDemos.Model.AdventureworksDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Keyword { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        private const int PageSize = 5;
        public StaticPagedList<Department> Departments { get;set; } = default!;

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
                .ToListAsync();

            Departments = new StaticPagedList<Department>(items, pageIndex + 1, PageSize, totalItemsCount);
        }
    }
}
