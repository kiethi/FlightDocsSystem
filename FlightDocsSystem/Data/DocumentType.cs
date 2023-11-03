using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { set; get; }
        [Required]
        public string Name { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? Note { get; set; }

        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        [Required]
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<Document>? Documents { get; set; }
        public virtual ICollection<TypeGroup>? TypeGroups { get; set; }


    }
}
