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
            if (id == null || id <= 0)
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
        public async Task<IActionResult> Create([Bind("Name,GroupName")] DepartmentDto department)
        {
            if (!string.IsNullOrWhiteSpace(department.Name)
                && _context.Departments.Any(d => d.Name == department.Name))
            {
                ModelState.AddModelError("", "Department name already exists");
            }

            if (ModelState.IsValid)
            {
                _context.Departments.Add(new Department { 
                  Name = department.Name,
                  GroupName= department.GroupName,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Department created successfully";
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || id <= 0)
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
                .FirstOrDefaultAsync(c => c.DepartmentId == id);
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
        public async Task<IActionResult> Edit(short id, [Bind("DepartmentId,Name,GroupName")] DepartmentDto departmentDto)
        {
            if (id != departmentDto.DepartmentId)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(departmentDto.Name)
                && _context.Departments.Any(d => d.DepartmentId != departmentDto.DepartmentId
                && d.Name == departmentDto.Name))
            {
                ModelState.AddModelError(nameof(departmentDto.Name), "Department name already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var deparment = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentId == departmentDto.DepartmentId);
                    if (deparment == null)
                    {
                        return NotFound();
                    }

                    deparment.Name = departmentDto.Name;
                    deparment.GroupName = departmentDto.GroupName;
                    deparment.ModifiedDate = DateTime.Now;
                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentDto.DepartmentId))
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

            TempData["Message"] = "Department updated successfully";
            return View(departmentDto);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || id <= 0)
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
