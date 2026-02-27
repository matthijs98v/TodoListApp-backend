using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoListRepository
{
    Task<List<TodoList>> GetAllTodoListsByUserIdAsync(int userId);
    Task AddAsync(TodoList todoList);
    Task DeleteAsync(int todoListId);
    Task UpdateAsync(int todoListId, TodoList todoList);
}
