using DMello.Application.Auth; //Now DMello.Api depends on Application Layer
using DMello.Application.Auth.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // <--- Inject Interface

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // 1. Set Short-Lived Access Token Cookie (15 mins in prod, 10s in test)
            Response.Cookies.Append("X-Access-Token", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddSeconds(10) // 10s testing window
            });

            // 2. Set Long-Lived Refresh Token Cookie (7 Days)
            Response.Cookies.Append("X-Refresh-Token", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh_Token_Login()
        {
            // 1. Extract raw refresh token from HttpOnly cookie
            var rawRefreshToken = Request.Cookies["X-Refresh-Token"];

            if (string.IsNullOrEmpty(rawRefreshToken))
            {
                return Unauthorized(new { message = "Refresh token cookie missing" });
            }

            // 2. Validate token against DB and generate new Access Token
            var response = await _authService.RefreshTokenAsync(rawRefreshToken);
            if (response == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            // 3. Append updated Access Token cookie to response
            Response.Cookies.Append("X-Access-Token", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddSeconds(10) // 10s for testing
            });

            return Ok(new { message = "Token Refreshed Successfully" });
        }


        #region New User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(result);
        }
        #endregion
    }
}
