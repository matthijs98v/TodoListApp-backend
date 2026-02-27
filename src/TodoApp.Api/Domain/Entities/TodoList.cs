using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Domain.Entities;

public class TodoList
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public required string Name {get; set;}
}
