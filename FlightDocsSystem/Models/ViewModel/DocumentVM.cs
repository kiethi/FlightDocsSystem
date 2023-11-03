using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ViewModel
{
    public class DocumentVM
    {
        public int DocumentId { get; set; }
        public string File { get; set; } = null!;

        public string DocumentName { get; set; } = null!;

        public string DocumentType { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string LatestVersion { get; set; } = string.Empty;
        public string CreatorName { get; set; } = null!;

        public string? Note { get; set; }

        public int FlightId { get; set; }

        public string FlightNo { get; set; } = null!;

        public string Route { get; set; } = null!;

        public DateTime DepartureDate { get; set; }
        public virtual ICollection<DocumentHistoryVM> DocumentHistoryVMs { get; set; } = new List<DocumentHistoryVM>();

        public class DocumentHistoryVM
        {
            public int DocumentHistoryId { get; set; }

            public string File { get; set; } = string.Empty;

            public string Version { get; set; } = string.Empty;

            public DateTime CreateDate { get; set; }
        }
    }
}
