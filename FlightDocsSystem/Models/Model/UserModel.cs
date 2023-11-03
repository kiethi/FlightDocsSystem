﻿using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.Model
{
    public class UserModel
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public Role Role { get; set; }

    }
}
