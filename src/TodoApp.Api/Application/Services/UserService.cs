using System.Text.RegularExpressions;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateUserAsync(User user)
    {

        // Do a email check first
        if ( !Regex.IsMatch(user.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") )
        {
            throw new ArgumentException("Invalid email format");
        }

        // Do a user check
        if ( !Regex.IsMatch(user.Name, @"^[a-zA-Z0-9]+$") )
        {
            throw new ArgumentException("Username must only contain letters and numbers");
        }

        // Do a check if user exists already by name
        var name = await _repository.CheckName(user.Name);
        if (name)
        {
            throw new ArgumentException("Username already exists");
        }

        // Do a check if user exists already by email
        var email = await _repository.CheckEmail(user.Email);
        if (email)
        {
            throw new ArgumentException("Email already exists");
        }

        await _repository.AddAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _repository.GetByIdAsync(userId);
    }

    public async Task<User> GetUserByNameAsync(string userName)
    {
        return await _repository.GetByNameAsync(userName);
    }

    public async Task<List<User>> SearchUserByNameAsync(string userName)
    {
        return await _repository.SearchByNameAsync(userName);
    }
}
