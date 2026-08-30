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

        #region Normal ShortTerm Token login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(response);
        }
        #endregion

        #region Refresh Token Login
        [HttpPost("login")]
        public async Task<IActionResult> Refresh_Token_Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Set JWT in HttpOnly Cookie
            Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            return Ok(new { email = response });// we will not send email as response response.Email 
        }

        #endregion

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
