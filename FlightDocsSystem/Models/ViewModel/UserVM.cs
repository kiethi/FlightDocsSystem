namespace FlightDocsSystem.Models.ViewModel
{
    public class UserVM
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
