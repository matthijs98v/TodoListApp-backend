using System;
using TodoApp.Api.Application.DTOs;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Interfaces;

public interface IListMemberRepository
{
    Task<List<ListMemberDto>> GetAllAsync(int todoListId);
    Task<ListMember> GetByIdAsync(int memberListId);
    Task AddAsync(ListMember listMember);
    Task DeleteAsync(int memberListId);
    Task<bool> CheckMember(int userId, int todoListId);
    Task<bool> CheckAdmin(int userId, int todoListId);
}
