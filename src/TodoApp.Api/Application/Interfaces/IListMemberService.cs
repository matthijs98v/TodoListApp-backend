using System;
using TodoApp.Api.Application.DTOs;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface IListMemberService
{
    Task<List<ListMemberDto>> GetUserListAsync(int userId, int todoListId);
    Task AddUserAsync(int userId, ListMember listMember);
    Task DeleteUserAsync(int userId, int memberListId);
}