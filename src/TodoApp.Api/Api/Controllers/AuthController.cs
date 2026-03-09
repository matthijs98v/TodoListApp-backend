using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Application.Interfaces;

namespace TodoApp.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }
 
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            try
            {
                string? AuthorizationHeader = HttpContext.Request.Headers.Authorization;

                var token = await _auth.Login(AuthorizationHeader);

                // Cookie base options
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(120)
                };
                
                // When production the cookie needs tobe secuere
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    cookieOptions.SameSite = SameSiteMode.Strict;
                    cookieOptions.Secure = true;
                } else
                {
                    cookieOptions.SameSite = SameSiteMode.Lax;
                    cookieOptions.Secure = false;
                }

                Response.Cookies.Append("Authorization", token, cookieOptions);
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UnixEpoch
            };

            // When production the cookie needs tobe secuere
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                cookieOptions.SameSite = SameSiteMode.Strict;
                cookieOptions.Secure = true;
            } else
            {
                cookieOptions.SameSite = SameSiteMode.Lax;
                cookieOptions.Secure = false;
            }

            Response.Cookies.Append("Authorization", "", cookieOptions);

            return Ok();
        }
    }
}
