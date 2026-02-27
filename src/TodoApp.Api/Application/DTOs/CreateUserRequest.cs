using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Application.DTOs;

public class CreateUserRequest
{
    [Required]
    [StringLength(128)]
    public required string Name { get; set; }

    [Required]
    [StringLength(128)]
    public required string Email { get; set; }

    [Required]
    public required string Password {get; set;}
}
