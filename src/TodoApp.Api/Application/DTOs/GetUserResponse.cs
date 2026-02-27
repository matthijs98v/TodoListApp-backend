using System;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.DTOs;

public class GetUserResponse
{
    public GetUserResponse (User user)
    {
        Id=user.Id;
        Name = user.Name;
        Email = user.Email;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email {get; set; }
}
