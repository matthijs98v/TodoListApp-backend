using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Application.DTOs;

public class CreateTodoListRequest
{
    [Required]
    [StringLength(128)]
    public required string Name {get; set;}
}
