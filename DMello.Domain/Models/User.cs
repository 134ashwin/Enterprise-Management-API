using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

//User.cs is a model class, it Represents a single user in the database and holds their data for Login,
// Forgot Password, and Refresh Tokens.
namespace DMello.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        //public string? ResetToken { get; set; }
        //public DateTime? ResetTokenExpiry { get; set; }
    }
}
