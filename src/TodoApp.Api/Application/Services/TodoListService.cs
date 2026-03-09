using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.SignalR;
using TodoApp.Api.Api.Hubs;
using TodoApp.Api.Application.DTOs;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class TodoListService : ITodoListService
{
    ITodoListRepository _repository;
    IListMemberRepository _listMemberRepository;
    IHubContext<TodoHub> _hubContext;
    public TodoListService(
        ITodoListRepository repository, 
        IListMemberRepository listMemberRepository,
        IHubContext<TodoHub> hubContext
    )
    {
        _repository = repository;
        _listMemberRepository = listMemberRepository;
        _hubContext = hubContext;
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

    public async Task<List<TodoListDTO>> GetAllTodoListsAsync(int userId)
    {
        var result = await _repository.GetAllTodoListsByUserIdAsync(userId);

        var todoLists = new List<TodoListDTO>();

        // Check all rows of creator or not
        foreach (var value in result)
        {
            bool isCreator = await _listMemberRepository.CheckAdmin(userId, value.Id);

            todoLists.Add(new TodoListDTO
            {
                Id=value.Id,
                Name=value.Name,
                IsCreator=isCreator
            });
        }

        // return 
        return todoLists;
    }

    public async Task<TodoListDTO> GetTodoListByIdAsync(int userId, int todoListId)
    {
        // Check rights
        bool hasRights = await _listMemberRepository.CheckMember(userId, todoListId);
        bool isCreator = await _listMemberRepository.CheckAdmin(userId, todoListId);

        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        var result = await  _repository.GetByIdAsync(todoListId);

        return new TodoListDTO
        {
            Id=result.Id,
            Name=result.Name,
            IsCreator=isCreator
        };

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
