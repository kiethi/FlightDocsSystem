using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? Note { get; set; }

        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        [Required]
        public virtual User Creator { get; set; } = null!;

        public virtual ICollection<GroupUser>? GroupUsers { get; set; } 

        public virtual ICollection<TypeGroup>? TypeGroups { get; set; }

    }
}
