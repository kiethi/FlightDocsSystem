namespace FlightDocsSystem.Models.EditModel
{
    public class FlightEM
    {
        public string FlightNo { get; set; } = string.Empty;
        public string Loading { get; set; } = string.Empty;
        public string Unloading { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
    }
}
