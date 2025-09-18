using EmployeeManagement.Api.Data;
using EmployeeManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Attendance
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Attendances.ToListAsync();
            return Ok(list);
        }

        // GET: api/Attendance/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _context.Attendances.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // GET: api/Attendance/ByEmployee/1
        [HttpGet("ByEmployee/{empId}")]
        public async Task<IActionResult> GetByEmployee(int empId)
        {
            var list = await _context.Attendances
                                     .Where(a => a.EmployeeId == empId)
                                     .ToListAsync();
            return Ok(list);
        }

        // POST: api/Attendance
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Attendance record)
        {
            _context.Attendances.Add(record);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = record.Id }, record);
        }

        // PUT: api/Attendance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Attendance record)
        {
            if (id != record.Id) return BadRequest();

            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Attendance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.Attendances.FindAsync(id);
            if (record == null) return NotFound();

            _context.Attendances.Remove(record);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
