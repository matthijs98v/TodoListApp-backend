using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;
using TodoApp.Api.Infrastructure.Data;

namespace TodoApp.Api.Infrastructure.Repositories;

public class ListMemberRepository : IListMemberRepository
{
    private readonly AppDbContext _context;
    public ListMemberRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(ListMember listMember)
    {
        await _context.ListMembers.AddAsync(listMember);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int memberListId)
    {
         var memberList = await _context.ListMembers
            .FirstOrDefaultAsync(x => x.Id == memberListId);

        if (memberList == null)
        {
            throw new KeyNotFoundException("MemberList not found");
        }

        _context.Remove(memberList);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync(int todoListId)
    {
        return await _context.ListMembers
            .Join(
                _context.Users,
                lm => lm.UserId, // Left side
                u => u.Id, // Rigth side
                (lm, u) => new { Users = u, lm.TodoListId }
            )
            .Where(x => x.TodoListId == todoListId)
            .Select(x => x.Users)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> CheckMember(int userId, int todoListId)
    {
        return await _context.ListMembers
            .Where(x => x.UserId == userId)
            .Where(x => x.TodoListId == todoListId)
            .Where(x => x.ReadOnly == false)
            .AnyAsync();
    }

    public async Task<bool> CheckAdmin(int userId, int todoListId)
    {
        return await _context.ListMembers
            .Where(x => x.UserId == userId)
            .Where(x => x.TodoListId == todoListId)
            .Where(x => x.ReadOnly == false)
            .Where(x => x.IsCreator == true)
            .AnyAsync();
    }
}
