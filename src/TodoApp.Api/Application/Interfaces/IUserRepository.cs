using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int userId);
    Task<User> GetByNameAsync(string userName);
    Task<User> GetByEmailAsync(string email);
    Task<List<User>> SearchByNameAsync(string userName);
    Task<bool> CheckName(string userName);
    Task<bool> CheckEmail(string email);
    Task<List<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
