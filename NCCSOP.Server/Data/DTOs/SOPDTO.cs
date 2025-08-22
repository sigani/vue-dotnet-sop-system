namespace NCCSOP.Server.Data.DTOs
{
    public class SOPItemFormDto
    {
        public int SopId {  get; set; }
        public string Content { get; set; }
        public int SortOrder { get; set; }
        public IFormFile Image { get; set; }
    }

    public class SOPItemJsonDto
    {
        public int SopId { get; set; }
        public string Content { get; set; }
        public int SortOrder { get; set; }
        // No file here
    }
}
