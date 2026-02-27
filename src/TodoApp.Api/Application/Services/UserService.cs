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
        Regex rx = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Match match = rx.Match(user.Email);
        if ( !match.Success )
        {
            throw new ArgumentException("Invailid email format");
        }

        // Do a check if user exists already by name
        var name = await _repository.CheckName(user.Name);
        if (name)
        {
            throw new ArgumentException("Name already exists");
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
}
