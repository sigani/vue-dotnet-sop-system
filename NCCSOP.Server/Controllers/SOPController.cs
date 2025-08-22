using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCCSOP.Server.Data;
using NCCSOP.Server.Models;
using NCCSOP.Server.Data.DTOs;

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

        [HttpPost("{id}")]
        public async Task<IActionResult> InsertSopItem(int id)
        {
            try
            {
                if (Request.HasFormContentType)
                {
                    // Handle form-data (with file)
                    var form = await Request.ReadFormAsync();

                    var dto = new SOPItemFormDto
                    {
                        SopId = int.Parse(form["SopId"]),
                        Content = form["Content"],
                        SortOrder = int.Parse(form["SortOrder"]),
                        Image = form.Files["Image"]
                    };

                    if (dto.Image != null)
                    {
                        var filePath = Path.Combine("uploads", dto.Image.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await dto.Image.CopyToAsync(stream);
                        }
                    }

                    // Save dto to DB, etc.
                    return Ok(new { message = "Handled form-data upload" });
                }
                else
                {
                    // Handle JSON
                    var dto = await System.Text.Json.JsonSerializer.DeserializeAsync<SOPItemJsonDto>(Request.Body);

                    // Save dto.Content, dto.SortOrder, etc.
                    return Ok(new { message = "Handled JSON request" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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