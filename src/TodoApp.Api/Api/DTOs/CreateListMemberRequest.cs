using System;

namespace TodoApp.Api.Api.DTOs;

public class CreateListMemberRequest
{
    public int UserId { get; set; }
    public int TodoListId { get; set; }
    public bool ReadOnly { get; set; }
}
