namespace FlightDocsSystem.Models.ViewModel
{
    public class DbDocumentVM
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public string FlightNo { get; set; } = null!;
        public DateTime DepartureDate { get; set; }
        public string Creator { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }

    }
}
