using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoItemService
{
    Task CreateTodoItemAsync(int userId, TodoItem todoItem);
    Task<List<TodoItem>> GetAllTodoItemsAsync(int userId, int todoListId);
    Task UpdateTodoItemAsync(int userId, int todoItemtId, TodoItem todoItem);
    Task DeleteTodoItemAsync(int userId, int todoItemtId);
}
