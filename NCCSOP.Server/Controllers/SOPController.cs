using Microsoft.AspNetCore.Mvc;
using NCCSOP.Server.Data;
using Microsoft.EntityFrameworkCore;

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

        // GET /sop/5
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
    }
}