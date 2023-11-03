using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ViewModel
{
    public class DocumentTypeVM
    {
        public int DocumentTypeId { set; get; }
        
        public string Name { get; set; } = null!;
        
        public DateTime CreatedDate { get; set; }

        public string CreatorName { get; set; } = null!;

        public int NoPermission { get; set; }
    }
}
