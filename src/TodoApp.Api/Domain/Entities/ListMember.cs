using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Api.Domain.Entities;

public class ListMember
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public int TodoListId { get; set; }

    [ForeignKey("TodoListId")]
    public TodoList? TodoList { get; set; }

    public bool IsCreator { get; set; }

    public bool ReadOnly { get; set; }
}
