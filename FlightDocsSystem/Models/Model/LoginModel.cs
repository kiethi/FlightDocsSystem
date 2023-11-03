using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.Model
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
