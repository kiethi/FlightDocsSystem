using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ViewModel
{
    public class FlightVM
    {
        public int FlightId { get; set; }
        public string FlightNo { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public int UserId { get; set; }
    }
}
