using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Api.DTOs;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListService _todoList;
        private readonly ITodoItemService _todoItem;
        public TodoListsController (
            ITodoListService todoList, 
            ITodoItemService todoItem
        )
        {
            _todoList = todoList;
            _todoItem = todoItem;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTodoLists()
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);

            var TodoLists = await _todoList.GetAllTodoListsAsync(userId);

            return Ok(new {TodoLists});
        }

        [HttpGet("{todoListId}")]
        [Authorize]
        public async Task<IActionResult> GetTodoListItems(int todoListId)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);

            var TodoListItems = await _todoItem.GetAllTodoItemsAsync(userId, todoListId);

            return Ok(new {TodoListItems});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTodoList(CreateTodoListRequest request)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");
            
            int userId = int.Parse(userIdClaim.Value);

            await _todoList.CreateTodoListAsync(userId, new TodoList
            {
                Name = request.Name
            });
            
            return Ok(new
            {
                Message="Todolist has been created"
            });
        }

        [HttpPut("{todoListId}")]
        [Authorize]
        public async Task<ActionResult> UpdateTodoList(int todoListId, CreateTodoListRequest request)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");
            
            int userId = int.Parse(userIdClaim.Value);

            await _todoList.UpdateTodoListAsync(userId, todoListId, new TodoList
            {
                Name = request.Name
            });

            return Ok(new
            {
                Message="Todolist has been updated"
            });
        }

        [HttpDelete("{todoListId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodoList(int todoListId)
        {
            // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");
            
            int userId = int.Parse(userIdClaim.Value);

            await _todoList.DeleteTodoListAsync(userId, todoListId);

            return Ok(new
            {
                Message="Todolist has been deleted"
            });
        }
    }
}
