using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Application.DTOs;

public class UpdatTodoItemRequest
{
    [StringLength(128)]
    public string? Title { get; set; }

    public int Status { get; set; }

    public int Order { get; set; }
}
