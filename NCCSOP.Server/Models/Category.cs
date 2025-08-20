using System.ComponentModel.DataAnnotations;

namespace NCCSOP.Server.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int ? ParentCategoryId { get; set; } = null;
        public Category? ParentCategory { get; set; } = null;

        // Navigation property for related SOPs
        public List<SOP> SOPs { get; set; } = new List<SOP>();
    }
}
