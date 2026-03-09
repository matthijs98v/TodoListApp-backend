using System;
using TodoApp.Api.Application.DTOs;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoListService
{
    Task CreateTodoListAsync(int userId, TodoList todoList);
    Task<List<TodoListDTO>> GetAllTodoListsAsync(int userId);
    Task<TodoListDTO> GetTodoListByIdAsync(int userId, int todoListId);
    Task UpdateTodoListAsync(int userId, int todoListId, TodoList todoList);
    Task DeleteTodoListAsync(int userId, int todoListId);
}
