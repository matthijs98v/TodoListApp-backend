using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoListService
{
    Task CreateTodoListAsync(int userId, TodoList todoList);
    Task<List<TodoList>> GetAllTodoListsAsync(int userId);
    Task UpdateTodoListAsync(int userId, int todoListId, TodoList todoList);
    Task DeleteTodoListAsync(int userId, int todoListId);
}
