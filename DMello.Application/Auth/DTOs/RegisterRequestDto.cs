using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Auth.DTOs
{
    public record RegisterRequestDto(string Email, string Password);
}
