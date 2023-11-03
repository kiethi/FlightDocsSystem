using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    public class TypeGroup
    {
        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; } = null!;

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; } = null!;

        public Permission Permission { get; set; } = Permission.NoPermission;

    }
}
