using System;
using System.Formats.Asn1;
using System.Security.Authentication;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class TodoListService : ITodoListService
{
    ITodoListRepository _repository;
    IListMemberRepository _listMemberRepository;
    public TodoListService(
        ITodoListRepository repository, 
        IListMemberRepository listMemberRepository
    )
    {
        _repository = repository;
        _listMemberRepository = listMemberRepository;
    }

    public async Task CreateTodoListAsync(int userId, TodoList todoList)
    {
        // Create todo list
        await _repository.AddAsync(todoList);

        // Asign to user
        await _listMemberRepository.AddAsync(new ListMember
        {
            UserId=userId,
            TodoListId=todoList.Id,
            ReadOnly=false,
            IsCreator=true
        });
    }

    public async Task DeleteTodoListAsync(int userId, int todoListId)
    {
        // Check if it is the admin or not
        bool hasRights = await _listMemberRepository.CheckAdmin(userId, todoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.DeleteAsync(todoListId);
    }

    public async Task<List<TodoList>> GetAllTodoListsAsync(int userId)
    {
        return await _repository.GetAllTodoListsByUserIdAsync(userId);
    }

    public async Task UpdateTodoListAsync(int userId, int todoListId, TodoList todoList)
    {
        // Check if it is the admin or not
        bool hasRights = await _listMemberRepository.CheckAdmin(userId, todoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.UpdateAsync(todoListId, todoList);
    }
}
