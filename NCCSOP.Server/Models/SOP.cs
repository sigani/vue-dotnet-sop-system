using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCCSOP.Server.Models
{
    public class SOP
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SOPItem> SOPItems { get; set; } = new List<SOPItem>();
        public int? CategoryId { get; set; } = null;
        [JsonIgnore]
        public Category? Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
