using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Api.DTOs.Request;

public class GetUserByNameRequest
{
    [StringLength(128)]
    public string? Name { get; set; }
}
