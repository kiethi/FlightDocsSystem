using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Data
{
    [Table("Flight")]
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [StringLength(255)]
        public string FlightNo { get; set; } = null!;

        public string Route { get; set; } = null!;

        public DateTime DepartureDate { get; set; }
        
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        [Required]
        public virtual User Creator { get; set; } = null!;

        public virtual ICollection<Document>? Documents { get; set; }
    }
}
