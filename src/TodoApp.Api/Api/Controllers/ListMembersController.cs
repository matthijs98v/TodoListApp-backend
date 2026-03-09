using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Api.DTOs.Response;
using TodoApp.Api.Api.DTOs.Request;

namespace TodoApp.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListMembersController : ControllerBase
    {
        private readonly IListMemberService _listMembers;
        public ListMembersController(IListMemberService listMembers)
        {
            _listMembers = listMembers;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddListMember(CreateListMemberRequest request)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);

            await _listMembers.AddUserAsync(userId, new ListMember
            {
                UserId=request.UserId,
                ReadOnly=request.ReadOnly,
                TodoListId=request.TodoListId,
                IsCreator=false,
            });

            return Ok(new
            {
                Message="List member has been created"
            });
        }

        [Authorize]
        [HttpDelete("{memberListId}")]
        public async Task<IActionResult> DeleteListMember(int memberListId)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);

            await _listMembers.DeleteUserAsync(userId, memberListId);

            return Ok(new
            {
                Message="List member has been deleted"
            });
        }

        [Authorize]
        [HttpGet("{todoListId}")]
        public async Task<IActionResult> GetAllListMembersOfTodoList(int todoListId)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);

            var listMembers = await _listMembers.GetUserListAsync(userId, todoListId);
             

            return Ok(new
            {
                listMembers
            });
        }
    }
}
