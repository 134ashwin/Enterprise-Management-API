using DMello.Application.Auth.DTOs;
using DMello.Application.Common.Interfaces;
using DMello.Domain.Interfaces; // Now Application is dependent on Domain
using DMello.Domain.Models;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


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

            // 3. Verify password hash safely
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null; // Wrong password
            }


            var token = _jwtService.GenerateToken(user);
            return new LoginResponseDto(token);
        }

        public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            // 1. Check if email already exists
            if (await _userRepo.IsEmailDuplicateAsync(request.Email))
            {
                return new RegisterResponseDto(false, "Email is already registered.");
            }

            // 2. Hash password securely
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Create User entity
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            // 4. Save to Database
            await _userRepo.AddAsync(user);

            return new RegisterResponseDto(true, "User registered successfully.");
        }
    }
}
