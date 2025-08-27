using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCCSOP.Server.Models
{
    public enum SOPItemType
    {
        Text,
        Image,
        Step
    }

    /// <summary>
    /// Represents a single item within a Standard Operating Procedure (SOP).
    /// </summary>
    public class SOPItem
    {
        /// <summary>
        /// The unique identifier for this SOP item.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The foreign key linking this item to its parent SOP.
        /// </summary>
        public int SOPId { get; set; }

        [JsonIgnore]
        /// <summary>
        /// The parent SOP that this item belongs to.
        /// </summary>
        public SOP SOP { get; set; } = null!;

        /// <summary>
        /// Name of step
        /// </summary>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// The main content of the item.  Basically the description.
        /// </summary>
        public string? Content { get; set; } = string.Empty;

        /// <summary>
        /// The image name.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// The step number if the item type is Step.
        /// Null for Text or Image types.
        /// </summary>
        public int? StepNumber { get; set; }

        /// <summary>
        /// Determines the display order of items within the SOP.
        /// Negative values can be used to indicate right-hand column placement (if using a 2-column layout).
        /// </summary>
        public int SortOrder { get; set; } = 0;
    }

    public class SOPUpdateItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }

        public int SortOrder { get; set; }
        public IFormFile? Image { get; set; }

    }
}
