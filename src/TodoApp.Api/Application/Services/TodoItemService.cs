using System;
using System.Security.Authentication;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class TodoItemService : ITodoItemService
{
    ITodoItemRepository _repository;
    IListMemberRepository _listMemberRepository;
    public TodoItemService(ITodoItemRepository repository, IListMemberRepository listMemberRepository)
    {
        _repository = repository;
        _listMemberRepository = listMemberRepository;
    }

    public async Task CreateTodoItemAsync(int userId, TodoItem todoItem)
    {
        // Check rights
        bool hasRights = await _listMemberRepository.CheckMember(userId, todoItem.TodoListId);

        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        // Insert todo item
        todoItem.Order = await _repository.GetCount(todoItem.TodoListId);
        await _repository.AddAsync(todoItem);
    }

    public async Task DeleteTodoItemAsync(int userId, int todoItemtId)
    {
        // Get todlist item model
        TodoItem todo = await _repository.GetById(todoItemtId);

        // Check if it is the admin or not
        bool hasRights = await _listMemberRepository.CheckMember(userId, todo.TodoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.DeleteAsync(todoItemtId);
    }

    public async Task<List<TodoItem>> GetAllTodoItemsAsync(int userId, int todoListId)
    {
        // Check if it is the admin or not
        bool hasRights = await _listMemberRepository.CheckMember(userId, todoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        return await _repository.GetByTodoListIdAsync(todoListId);
    }

    public async Task UpdateTodoItemAsync(int userId, int todoItemtId, TodoItem todoItem)
    {
        bool hasRights = await _listMemberRepository.CheckMember(userId, todoItem.TodoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }
        
        await _repository.UpdateAsync(todoItemtId, todoItem);
    }
}
