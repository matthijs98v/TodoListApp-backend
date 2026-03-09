using System;

namespace TodoApp.Api.Application.DTOs;

public class ListMemberDto
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
    public bool IsCreator { get; set; }
}