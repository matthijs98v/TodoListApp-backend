using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface ITodoItemRepository
{
    Task<List<TodoItem>> GetByTodoListIdAsync(int todoListId);
    Task<TodoItem> GetByIdAsync(int todoItemId);
    Task<int> GetCount(int todoListId);
    Task<TodoItem> AddAsync(TodoItem todoList);
    Task DeleteAsync(int todoListId);
    Task UpdateAsync(int todoListId, TodoItem todoList);
}
