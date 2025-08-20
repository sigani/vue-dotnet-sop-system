using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCCSOP.Server.Data;
using NCCSOP.Server.Models;

namespace NCCSOP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _db.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SOPs)
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _db.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SOPs)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null)
                return BadRequest();

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category updatedCategory)
        {
            if (updatedCategory == null || updatedCategory.Id != id)
                return BadRequest();

            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;
            category.ParentCategoryId = updatedCategory.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}