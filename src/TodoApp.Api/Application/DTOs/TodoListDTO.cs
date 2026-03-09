using System;

namespace TodoApp.Api.Application.DTOs;

public class TodoListDTO
{
    public int Id { get; set; }
    public required string Name {get; set;}
    public bool IsCreator {get; set;}
}
