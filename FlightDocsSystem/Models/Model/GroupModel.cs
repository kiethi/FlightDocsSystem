using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.Model
{
    public class GroupModel
    {
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public int CreatorId { get; set; }
        public int[]? UserIds { get; set; }

    }
}
