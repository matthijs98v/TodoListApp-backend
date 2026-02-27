using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoItemRepository
{
    Task<List<TodoItem>> GetByTodoListIdAsync(int todoListId);
    Task<TodoItem> GetById(int todoItemId);
    Task AddAsync(TodoItem todoList);
    Task DeleteAsync(int todoListId);
    Task UpdateAsync(int todoListId, TodoItem todoList);
}
