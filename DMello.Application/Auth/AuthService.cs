using DMello.Application.Auth.DTOs;
using DMello.Application.Common.Interfaces;
using DMello.Domain.Interfaces;
using DMello.Domain.Models;
using System;
using System.Threading.Tasks;

namespace DMello.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return null; // Email not found
            }

            // Verify password hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null; // Wrong password
            }

            // 1. Generate Tokens
            var accessToken = _jwtService.GenerateToken(user);
            var rawRefreshToken = _jwtService.GenerateRefreshToken(); // Uses RandomNumberGenerator

            // 2. Save Refresh Token to Database
            user.RefreshToken = rawRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepo.UpdateAsync(user);

            // 3. Return DTO with both parameters
            return new LoginResponseDto(accessToken, rawRefreshToken);
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string incomingRawRefreshTokenSentByBrowser) // sent via HttpONly Cookiee
        {
            // 1. Finding a user by RefreshToken in database, so that we can delete existing refresh token and generate new one after few days
            var user = await _userRepo.GetByRefreshTokenAsync(incomingRawRefreshTokenSentByBrowser);

            // 2. Validate token existence and expiry date stored in DB
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null; // Invalid or expired refresh token
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(incomingRawRefreshTokenSentByBrowser, user.RefreshToken);
            if (!isValid)
            {
                return null; // Stolen or manipulated token
            }

            // 3. Generate NEW Access Token AND NEW Refresh Token (Token Rotation)
            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // 4. Save NEW Refresh Token back to DB
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepo.UpdateAsync(user);

            return new LoginResponseDto(newAccessToken, newRefreshToken);
        }

        public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepo.IsEmailDuplicateAsync(request.Email))
            {
                return new RegisterResponseDto(false, "Email is already registered.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepo.AddAsync(user);

            return new RegisterResponseDto(true, "User registered successfully.");
        }
    }
}