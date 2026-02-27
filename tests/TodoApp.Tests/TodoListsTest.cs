using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Application.Common.Utilities;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Application.Services;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Infrastructure.Data;
using TodoApp.Api.Infrastructure.Repositories;

namespace TodoApp.Tests;

public class TodoListsTest
{
    private IUserService _userSerivice;
    public TodoListsTest()
    {
        // Setup the inmomory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        // Seed the database
        var context = new AppDbContext(options);

        // Setup the user service
        var userRepository = new UserRepository(context);
        _userSerivice = new UserService(userRepository);

        // Arrange: Seed data into the context
        _userSerivice.CreateUserAsync(new User
        {
            Id= 1,
            Name = "Test",
            Email = "test1@test.com",
            Password = PasswordHasher.Hash("Welkom01")
        });

        _userSerivice.CreateUserAsync(new User
        {
            Id= 2,
            Name = "Test2",
            Email = "test2@test.com",
            Password = PasswordHasher.Hash("Welkom01")
        });
    }

    [Fact]
    public async Task Test_CreateUsers_ShouldReturn2()
    {
        var result = await _userSerivice.GetAllUsersAsync();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Test_CreateUsersWithNameThatAlreadyExists_ShouldFail()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => {
            await  _userSerivice.CreateUserAsync(new User
                {
                    Name = "Test2",
                    Email = "test99@test.com",
                    Password = PasswordHasher.Hash("Welkom01")
                });
        });
    }

    [Fact]
    public async Task Test_CreateUsersWithEmailThatAlreadyExists_ShouldFail()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => {
            await  _userSerivice.CreateUserAsync(new User
                {
                    Name = "Test99",
                    Email = "test2@test.com",
                    Password = PasswordHasher.Hash("Welkom01")
                });
        });
    }

    [Fact]
    public async Task Test_CreateUsersWithEmailThatInvailid_ShouldFail()
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => {
            await  _userSerivice.CreateUserAsync(new User
                {
                    Name = "Test99",
                    Email = "test2",
                    Password = PasswordHasher.Hash("Welkom01")
                });
        });
    }
}
