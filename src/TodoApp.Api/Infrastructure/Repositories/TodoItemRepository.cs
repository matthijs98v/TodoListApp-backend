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

    public async Task<TodoItem> AddAsync(TodoItem todoItem)
    {
        _context.Todos.Add(todoItem);
        await _context.SaveChangesAsync();

        return todoItem;
    }

    public async Task DeleteAsync(int todoId)
    {
        var todoItem = await _context.Todos
            .FirstOrDefaultAsync(x => x.Id == todoId) 
            ?? throw new KeyNotFoundException("TodoItem not found");

        _context.Remove(todoItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoItem> GetByIdAsync(int todoItemId)
    {
        var result = await _context.Todos
            .FirstOrDefaultAsync(x => x.Id == todoItemId) 
            ?? throw new KeyNotFoundException("TodoItem not found");
            
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
        var todo = await _context.Todos
        .FirstOrDefaultAsync(x => x.Id == todoItemId) 
        ?? throw new KeyNotFoundException("TodoList not found");
        
        if (todoItem.Title != null)
        {
            todo.Title = todoItem.Title;
        }

        if (todoItem.Status.HasValue)
        {
            todo.Status = todoItem.Status;
        }

        if (todoItem.Order.HasValue)
        {
            // Get all items by id
            var todoItemList= await _context.Todos
                .Where(x => x.TodoListId == todo.TodoListId)
                .OrderBy(x => x.Order)
                .ToListAsync();

            int? oldOrder = todo.Order;
            int newOrder = todoItem.Order.Value;

            // Do the sorting
            foreach (TodoItem item in todoItemList)
            {
                if (item.Id == todo.Id)
                    continue;

                if (newOrder > oldOrder)
                {
                    if (item.Order > oldOrder && item.Order <= newOrder)
                        item.Order--;
                }
                else if (newOrder < oldOrder)
                {
                    if (item.Order >= newOrder && item.Order < oldOrder)
                        item.Order++;
                }
                
            }

            todo.Order = newOrder;
        }

        await _context.SaveChangesAsync();
    }
}
