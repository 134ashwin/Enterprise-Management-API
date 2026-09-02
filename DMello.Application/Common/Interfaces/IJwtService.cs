using DMello.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Common.Interfaces
{
    public interface IJwtService
    {
        // Generates a signed JWT bearer token string for an authenticated user
        string GenerateToken(User user);

        string GenerateRefreshToken();
    }
}
