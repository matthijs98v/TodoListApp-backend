using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Infrastructure.Data;

namespace TodoApp.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(User item)
    {
        _context.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FirstAsync(x => x.Id == id);
    }

    public async Task<User> GetByNameAsync(string Name)
    {
        return await _context.Users.FirstAsync(x => x.Name == Name);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstAsync(x => x.Email == email);
    }

    public Task UpdateAsync(User item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckName(string userName)
    {
        return await _context.Users.AnyAsync(x => x.Name == userName);
    }

    public async Task<bool> CheckEmail(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    
}
