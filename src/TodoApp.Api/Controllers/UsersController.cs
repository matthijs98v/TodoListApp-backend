using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Application.Common.Utilities;
using TodoApp.Api.Application.DTOs;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;
        public UsersController (IUserService users)
        {
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _users.GetAllUsersAsync();

            // User dto mapping to strip out the password
            var result = users
                .Select(x => new GetUserResponse(x))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var user = await _users.GetUserByIdAsync(Id);

            // User dto mapping to strip out the password
            var result = new GetUserResponse(user);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest Request)
        {
            var User = new User
            {
                Name=Request.Name,
                Email=Request.Email,
                Password=PasswordHasher.Hash(Request.Password)
            };

            await _users.CreateUserAsync(User);

            return Ok(new
            {
                Success=true,
                Message="User has been created"
            });
        }

        public async Task<IActionResult> GetUserByIdAsync(int Id)
        {
            var user =  await _users.GetUserByIdAsync(Id);

            // User dto mapping to strip out the password
            var result = new GetUserResponse(user);

            return Ok(result);
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return BadRequest("Id is required");

            var user = await _users.GetUserByIdAsync(int.Parse(userIdClaim.Value));

            // User dto mapping to strip out the password
            var result = new GetUserResponse(user);

            return Ok(result);
        }
    }
}
