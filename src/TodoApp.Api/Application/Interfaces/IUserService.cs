using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(User user);

    Task<List<User>> GetAllUsersAsync();

    Task<User> GetUserByIdAsync(int userId);

    Task<User> GetUserByNameAsync (string userId);
}
