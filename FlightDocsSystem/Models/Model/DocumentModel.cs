namespace FlightDocsSystem.Models.Model
{
    public class DocumentModel
    {
        public IFormFile File { get; set; } = null!;
        public int DocumentTypeId { get; set; }
        public string? Note { get; set; } = string.Empty;
        public int CreatorId { get; set; }
        public int FlightId { get; set; }

    }
}
