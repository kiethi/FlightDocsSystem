using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.Model
{
    public class DocumentTypeModel
    {
        public string Name { get; set; } = null!;

        public int CreatorId { get; set; }

        public string? Note { get; set; }

        public int[]? GroupIds { get; set; }
    }
}
