using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Auth.DTOs
{
    public record LoginRequestDto // Guys best Approach for DTOs 
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
