using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public required string Name { get; set; }

    [StringLength(128)]
    public required string Email { get; set; }

    public required string Password {get; set;}
}
