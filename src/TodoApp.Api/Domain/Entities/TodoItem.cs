using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Api.Domain.Entities;

public class TodoItem
{
    [Key]
    public int Id { get; set; }

    public int TodoListId {get; set;}

    [ForeignKey("TodoListId")]
    public TodoList? TodoList { get; set; }

    [StringLength(128)]
    public string? Title { get; set; }

    public int? Status { get; set; }

    public int? Order { get; set; }
}
