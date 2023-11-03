using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ViewModel
{
    public class GroupVM
    {
        public int GroupId { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string? Note { get; set; }
    }
}
