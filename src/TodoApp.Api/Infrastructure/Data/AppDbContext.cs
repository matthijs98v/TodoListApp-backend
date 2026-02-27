using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<TodoItem> Todos { get; set; }

    public DbSet<TodoList> TodoLists { get; set; }

    public DbSet<ListMember> ListMembers { get; set; }
}
