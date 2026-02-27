using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Application.Interfaces;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IConfiguration _config;
        public AuthController(IAuthService auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            try
            {
                string? Cookie = HttpContext.Request.Cookies["Authorization"];
                string? AuthorizationHeader = HttpContext.Request.Headers.Authorization;

                var token = await _auth.Login(Cookie ?? AuthorizationHeader);

                // Create cookie
                Response.Cookies.Append("Authorization", token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(120)
                });

                return Ok();
            } catch (Exception)
            {
                return BadRequest(
                    new
                    {
                        Message="Login failed"
                    }
                );
            }
          
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Append("Authorization", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddSeconds(0)
            });

            return Ok();
        }
    }
}
