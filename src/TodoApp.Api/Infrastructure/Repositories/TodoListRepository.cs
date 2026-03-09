using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Infrastructure.Data;

namespace TodoApp.Api.Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly AppDbContext _context;
    public TodoListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TodoList item)
    {
        _context.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int todoListId, TodoList item)
    {
        var todoList = await _context.TodoLists
            .FirstOrDefaultAsync(x => x.Id == todoListId) 
            ?? throw new KeyNotFoundException("TodoList not found");

        todoList.Name = item.Name;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var todoList = await _context.TodoLists
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new KeyNotFoundException("TodoList not found");
            
        _context.Remove(todoList);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TodoList>> GetAllTodoListsByUserIdAsync(int userId)
    {
        return await _context.TodoLists
            .Join(
                _context.ListMembers,
                tl => tl.Id, // Left side
                lm => lm.TodoListId, // Rigth side
                (tl, lm) => new { TodoList = tl, lm.UserId }
            )
            .Where(x => x.UserId == userId)
            .Select(x => x.TodoList)
            .Distinct()
            .ToListAsync();
    }

    public async Task<TodoList> GetByIdAsync(int todoListId)
    {
        var todoList = await _context.TodoLists
            .FirstOrDefaultAsync(x => x.Id == todoListId)
            ?? throw new KeyNotFoundException("TodoList not found");

        return todoList;
    }
}
