using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCCSOP.Server.Data;
using NCCSOP.Server.Models;

namespace NCCSOP.Server.Controllers
{
    [ApiController]
    [Route("api/sop")]
    public class SOPController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SOPController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSOPs()
        {
            var sop = await _db.SOPs
                .Include(s => s.SOPItems)
                .ToListAsync();

            if(sop == null)
            {
                return NotFound();
            }

            return Ok(sop);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SOP sop)
        {
            System.Diagnostics.Debug.WriteLine("hello??");
            if (sop == null)
                return BadRequest();

            sop.CreatedAt = DateTime.UtcNow;
            sop.UpdatedAt = DateTime.UtcNow;

            _db.SOPs.Add(sop);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSOP), new { id = sop.Id }, sop);
        }


        // GET api/sop/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSOP(int id)
        {
            var sop = await _db.SOPs
                .Include(s => s.SOPItems)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sop == null)
                return NotFound();

            return Ok(sop);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sop = await _db.SOPs.FindAsync(id);
            if (sop == null)
                return NotFound();

            _db.SOPs.Remove(sop);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}