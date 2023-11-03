using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    public class DocumentHistory
    {
        [Key]
        public int DocumentHistoryId { get; set; }

        public string File { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        [Required]
        public virtual Document Document { get; set; } = null!;

        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        [Required]
        public virtual User Creator { get; set; } = null!;

    }
}
