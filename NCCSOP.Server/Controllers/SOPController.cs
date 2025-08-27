using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCCSOP.Server.Data;
using NCCSOP.Server.Models;
using NCCSOP.Server.Data.DTOs;
using System.Diagnostics;

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
            Debug.WriteLine("hello??");
            if (sop == null)
                return BadRequest();

            sop.CreatedAt = DateTime.UtcNow;
            sop.UpdatedAt = DateTime.UtcNow;

            _db.SOPs.Add(sop);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSOPDetails), new { id = sop.Id }, sop);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> InsertSopItem(int id, [FromForm] SOPItemDto dto)
        {
            if(string.IsNullOrEmpty(dto.Name) && string.IsNullOrEmpty(dto.Content) && dto.Image == null)
            {
                return BadRequest(new { message = "At least one field must be populated" });
            }

            try
            {
                var filePath = "";
                var fileExtension = "";
                var uniqueFileName = "";
                if (dto.Image != null)
                {
                    // Handle form-data
                    fileExtension = Path.GetExtension(dto.Image.FileName);
                    uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var uploadsFolder = Path.Combine("uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await dto.Image.CopyToAsync(stream);
                }

                var item = new SOPItem
                {
                    SOPId = dto.SopId,
                    Name = dto.Name,
                    Content = dto.Content,
                    SortOrder = dto.SortOrder,
                    ImagePath = uniqueFileName
                };

                await _db.SOPItems.AddAsync(item);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Item successfully created", savedItem = item });
              
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var path = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileExt = Path.GetExtension(fileName).ToLower();
            var contentType = fileExt switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, contentType);
        }

        // GET api/sop/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSOPDetails(int id)
        {
            var sop = await _db.SOPs
                .Include(s => s.SOPItems.OrderBy(i => i.SortOrder))
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sop == null)
                return NotFound();

            return Ok(sop);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SOP updatedSOP)
        {
            if (updatedSOP == null || id != updatedSOP.Id)
            {
                return BadRequest();
            }
            var existingSOP = await _db.SOPs.FindAsync(id);
            if (existingSOP == null)
                return NotFound();
            existingSOP.Name = updatedSOP.Name;
            existingSOP.CategoryId = updatedSOP.CategoryId;
            existingSOP.SOPItems = updatedSOP.SOPItems;
            existingSOP.UpdatedAt = DateTime.UtcNow;
            _db.SOPs.Update(existingSOP);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("item/{id}")]
        public async Task<IActionResult> UpdateSOPItem(int id, [FromForm] SOPUpdateItemDto updatedItem)
        {
            try
            {
                if (updatedItem == null || id != updatedItem.Id)
                    return BadRequest();
                var existingItem = await _db.SOPItems.FindAsync(id);
                if (existingItem == null)
                    return NotFound();
                existingItem.Name = updatedItem.Name;
                existingItem.Content = updatedItem.Content;
                existingItem.SortOrder = updatedItem.SortOrder;
                System.Diagnostics.Debug.WriteLine(existingItem.ImagePath + " new image: " + updatedItem.ImagePath);
                if (existingItem.ImagePath != updatedItem.ImagePath)
                {
                    if (!string.IsNullOrEmpty(existingItem.ImagePath))
                    {
                        var uploadsFolder = Path.Combine("uploads");
                        var oldFilePath = Path.Combine(uploadsFolder, existingItem.ImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                            catch (Exception ex)
                            {
                                // Optional: log the error
                                Console.WriteLine($"Failed to delete old image: {ex.Message}");
                            }
                        }
                    }

                    // only upload a photo if they had a photo to upload.
                    // maybe they deleted a photo who knows
                    if (!string.IsNullOrEmpty(updatedItem.ImagePath))
                    {
                        var filePath = "";
                        var fileExtension = "";
                        var uniqueFileName = "";
                        var uploadsFolder = Path.Combine("uploads");
                        if (updatedItem.Image != null)
                        {
                            // Handle form-data
                            fileExtension = Path.GetExtension(updatedItem.Image.FileName);
                            uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                            uploadsFolder = Path.Combine("uploads");
                            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                            filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await updatedItem.Image.CopyToAsync(stream);
                        }
                        existingItem.ImagePath = uniqueFileName;
                    } else
                    {
                        existingItem.ImagePath = "";
                    }
                }

                _db.SOPItems.Update(existingItem);
                await _db.SaveChangesAsync();
                return NoContent();
            } catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            
        }

        [HttpDelete("{sopId}/{detailId}")]
        public async Task<IActionResult> DeleteDetail(int sopId, int detailId)
        {
            // Find the SOPItem with the matching SOPId and Id
            var item = await _db.SOPItems
                .FirstOrDefaultAsync(i => i.SOPId == sopId && i.Id == detailId);

            if (item == null)
                return NotFound("SOP item not found");

            // Delete the image file from disk
            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                var uploadsFolder = Path.Combine("uploads"); // same folder used in POST
                var filePath = Path.Combine(uploadsFolder, item.ImagePath);

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Optional: log the error
                        Console.WriteLine($"Failed to delete image: {ex.Message}");
                    }
                }
            }

            _db.SOPItems.Remove(item);
            await _db.SaveChangesAsync();

            return Ok(new { message = "SOP item deleted successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sop = await _db.SOPs.FindAsync(id);
            if (sop == null)
                return NotFound();

            // Delete associated images
            var uploadsFolder = Path.Combine("uploads"); // match the folder used when saving
            foreach (var item in sop.SOPItems)
            {
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    var filePath = Path.Combine(uploadsFolder, item.ImagePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            // Optional: log error but continue deleting other files
                            Console.WriteLine($"Failed to delete image {item.ImagePath}: {ex.Message}");
                        }
                    }
                }
            }

            _db.SOPs.Remove(sop);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}