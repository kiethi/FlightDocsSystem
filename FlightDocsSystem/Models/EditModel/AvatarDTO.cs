using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.EditModel
{
    public class AvatarDTO
    {
        public int UserId { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
