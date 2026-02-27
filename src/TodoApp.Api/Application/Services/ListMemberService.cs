using System.Security.Authentication;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class ListMemberService : IListMemberService
{
    IListMemberRepository _repository;
    public ListMemberService(IListMemberRepository repository)
    {
        _repository = repository;
    }
    public async Task AddUserAsync(int userId, ListMember listMember)
    {
        // Check rights
        bool hasRights = await _repository.CheckMember(userId, listMember.TodoListId);

        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.AddAsync(listMember);
    }

    public async Task<List<User>> GetUserListAsync(int userId, int todoListId)
    {
        // Check rights
        bool hasRights = await _repository.CheckMember(userId, todoListId);

        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        return await _repository.GetAllAsync(todoListId);
    }

    public async Task DeleteUserAsync(int userId, int memberListId, int todoListId)
    {
        // Check rights
        bool hasRights = await _repository.CheckAdmin(userId, todoListId);

        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.DeleteAsync(memberListId);
    }
}
