namespace NCCSOP.Server.Data.DTOs
{
    public class SOPItemDto
    {
        public int SopId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public int SortOrder { get; set; }
        public IFormFile? Image { get; set; }
    }
}
