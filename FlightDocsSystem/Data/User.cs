using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    [Table("User")]
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public byte[]? Avatar { get; set; }

        public Role Role { get; set; } 

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<GroupUser>? GroupUsers { get; set; }
        public virtual ICollection<Flight>? Flights { get; set; }
        public virtual ICollection<DocumentHistory>? DocumentHistories { get; set; }

    }
}
