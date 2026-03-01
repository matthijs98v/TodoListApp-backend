using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Api.DTOs;

public class CreateTodoItemRequest
{
    public int TodoListId {get; set;}

    [StringLength(128)]
    [Required]
    public required string Title { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public int Order { get; set; }
}
