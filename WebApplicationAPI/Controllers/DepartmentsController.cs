using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Model;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AdventureworksDbContext _context;

        public DepartmentsController(AdventureworksDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            return await _context.Departments
                .Select(c => new DepartmentDto(c.DepartmentId, c.Name, c.GroupName))
                .ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(short id)
        {
            var department = await _context.Departments
                .Select(c => new { 
                  c.DepartmentId,
                  c.Name,
                  c.GroupName
                })
                .FirstOrDefaultAsync(c => c.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            return new DepartmentDto(department.DepartmentId
                , department.Name
                , department.GroupName);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutDepartment(short id, DepartmentDto departmentDto)
        {
            if (id != departmentDto.DepartmentId)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = departmentDto.Name;
            department.GroupName = departmentDto.GroupName;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartmentDto>> PostDepartment(DepartmentDto departmentDto)
        {
            var department = new Department
            {
                Name = departmentDto.Name,
                GroupName = departmentDto.GroupName
            };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, new DepartmentDto(department.DepartmentId
                , department.Name
                , department.GroupName));
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteDepartment(short id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    public record DepartmentDto(int DepartmentId, string Name, string GroupName);
}
