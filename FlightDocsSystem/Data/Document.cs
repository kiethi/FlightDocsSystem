using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        [StringLength(255)]
        public string DocumentName { get; set; } = null!;

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        [Required]
        public virtual DocumentType DocumentType { get; set; } = null!;

        public string LatestVersion { get; set; } = string.Empty;

        public string? Note { get; set; }

        public int FlightId { get; set; }

        [ForeignKey("FlightId")]
        [Required]
        public virtual Flight Flight { get; set; } = null!;

        public virtual ICollection<DocumentHistory> DocumentHistories { get; set; } = new List<DocumentHistory>();

    }
}
