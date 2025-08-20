using System.ComponentModel.DataAnnotations;

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
    /// An SOPItem can be a text block, an image, or a step, and can be ordered within the SOP.
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

        /// <summary>
        /// The parent SOP that this item belongs to.
        /// </summary>
        public SOP SOP { get; set; } = null!;

        /// <summary>
        /// The type of the SOP item (Text, Image, or Step).
        /// </summary>
        public SOPItemType Type { get; set; } = SOPItemType.Text;

        /// <summary>
        /// The main content of the item.
        /// For Text or Step types, this contains the textual description.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The URL or file path of an image, if the item type is Image.
        /// Null for Text or Step types.
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
}
