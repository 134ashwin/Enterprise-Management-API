using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize] // Requires valid JWT cookie/header
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new { email = User.Identity?.Name ?? "User", status = "Active" });
        }
    }
}
