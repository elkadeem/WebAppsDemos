using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWebAppDemo.Entities;
using MVCWebAppDemo.Model;
using MVCWebAppDemo.Models;
using X.PagedList;

namespace MVCWebAppDemo.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AdventureworksDbContext _context;
        private const int PageSize = 5;
        public DepartmentsController(AdventureworksDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(DepartmentsViewModel viewModel)
        {
            IQueryable<Department> query = _context.Departments;
            if (!string.IsNullOrWhiteSpace(viewModel.Keyword))
            {
                query = query.Where(p => p.Name.Contains(viewModel.Keyword));
            }

            int pageIndex = viewModel.CurrentPage - 1;
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

            viewModel.Departments = new StaticPagedList<DepartmentDto>(items, pageIndex + 1, PageSize, totalItemsCount);
                      
            return View(viewModel);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(short? id)
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

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name,GroupName,ModifiedDate")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("DepartmentId,Name,GroupName,ModifiedDate")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(short id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
