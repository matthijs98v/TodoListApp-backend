using System;

namespace TodoApp.Api.Application.Interfaces;

public interface IAuthService
{
   Task<string> Login(string? authorizationString);
}
