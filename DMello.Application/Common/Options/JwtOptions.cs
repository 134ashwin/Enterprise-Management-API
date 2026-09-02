using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Common.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryInSeconds { get; init; } = 10; // Expiry Minitues set to 30 sec so that refresh token gets active
    }
}
