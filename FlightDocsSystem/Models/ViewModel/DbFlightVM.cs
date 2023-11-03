using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ViewModel
{
    public class DbFlightVM
    {
        public int FlightId { get; set; }

        public string FlightNo { get; set; } = null!;

        public string Route { get; set; } = null!;

        public DateTime DepartureDate { get; set; }
    }
}
