using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    public class GroupUser
    {
        public int GroupId { get; set; }    

        [ForeignKey("GroupId")] 
        public virtual Group Group { get; set; } = null!;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
