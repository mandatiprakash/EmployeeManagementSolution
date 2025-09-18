
using EmployeeManagement.Api.Data;
using EmployeeManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Departments.ToListAsync();
            return Ok(list);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            return Ok(dept);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department dept)
        {
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dept.Id }, dept);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Department dept)
        {
            if (id != dept.Id) return BadRequest();

            _context.Entry(dept).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
