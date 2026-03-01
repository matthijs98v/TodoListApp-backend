using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Infrastructure.Data;

namespace TodoApp.Api.Infrastructure.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly AppDbContext _context;
    public TodoItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TodoItem todoList)
    {
        _context.Todos.Add(todoList);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int todoId)
    {
        var todoItem = await _context.Todos
            .FirstOrDefaultAsync(x => x.Id == todoId);

        if (todoItem == null)
        {
            throw new KeyNotFoundException("TodoItem not found");
        }

        _context.Remove(todoItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoItem> GetById(int todoItemId)
    {
        var result = await _context.Todos
            .FirstOrDefaultAsync(x => x.Id == todoItemId);

        if (result == null)
        {
            throw new KeyNotFoundException("TodoItem not found");
        }

        return result;
    }

    public async Task<int> GetCount(int todoListid)
    {
        return await _context.Todos
            .Where(x => x.TodoListId == todoListid)
            .CountAsync();
    }

    public async Task<List<TodoItem>> GetByTodoListIdAsync(int todoListId)
    {
        return await _context.Todos
            .Where(x => x.TodoListId == todoListId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task UpdateAsync(int todoItemId, TodoItem todoItem)
    {
        var todo= await _context.Todos
        .FirstOrDefaultAsync(x => x.Id == todoItemId);

        if (todo == null)
        {
            throw new KeyNotFoundException("TodoList not found");
        }

        todo.Title = todoItem.Title ?? todo.Title;
        todo.Status = todoItem.Status ?? todo.Status;
        todo.Order = todoItem.Order ?? todo.Order;

        await _context.SaveChangesAsync();
    }
}
