namespace FlightDocsSystem.Models.EditModel
{
    public class DocumentEM
    {
        public IFormFile File { get; set; } = null!;
        public int DocumentId { get; set; } 
        public int CreatorId { get; set; }
    }
}
