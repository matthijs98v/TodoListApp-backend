using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Api.DTOs;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItem;
        public TodoItemsController(ITodoItemService todoItem)
        {
            _todoItem = todoItem;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTodo(CreateTodoItemRequest request)
        {   
             // Get the user id
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);
            
            await _todoItem.CreateTodoItemAsync(userId, new TodoItem
            {
                TodoListId=request.TodoListId,
                Title=request.Title,
                Status=request.Status,
                Order=request.Order
            });

            return Ok(new {
                Message="Todo item has been created"
            });
        }

        [HttpPut("{todoItemId}")]
        [Authorize]
        public async Task<IActionResult> UpdatTodo(int todoItemId, UpdatTodoItemRequest request)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return BadRequest("Id is required");

            int userId = int.Parse(userIdClaim.Value);
            
            await _todoItem.UpdateTodoItemAsync(userId, todoItemId,  new TodoItem
            {
                Title=request.Title,
                Status=request.Status,
                Order=request.Order
            });

            return Ok(new {
                Message="Todo item has been updated"
            });
        }

        [HttpDelete("{todoItemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTodo(int todoItemId)
        {
             return Ok(new {
                Message="Todo item has been deleted"
            });
        }
    }
}
