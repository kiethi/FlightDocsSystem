using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.Model
{
    public class FlightModel
    {
        public string FlightNo { get; set; } = string.Empty;
        public string Loading { get; set; } = string.Empty;
        public string Unloading { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public int CreatorId { get; set; }
    }
}
