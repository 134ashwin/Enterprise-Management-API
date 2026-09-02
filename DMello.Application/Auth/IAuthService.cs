using DMello.Application.Auth.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Application.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request);
    }
}