using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.SignalR;
using TodoApp.Api.Api.Hubs;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class TodoItemService : ITodoItemService
{
    ITodoItemRepository _repository;
    IListMemberRepository _listMemberRepository;
    IHubContext<TodoHub> _hubContext;
    public TodoItemService(
        ITodoItemRepository repository, 
        IListMemberRepository listMemberRepository,
        IHubContext<TodoHub> hubContext
    )
    {
        _repository = repository;
        _listMemberRepository = listMemberRepository;
        _hubContext = hubContext;
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
        var todoItemModel = await _repository.AddAsync(todoItem);


        // Send when todo is updated
        await _hubContext.Clients.Group("todoList"+todoItemModel.TodoListId).SendAsync("ReceiveMessage", new
        {
            Method="loadTodoItems"
        });
    }

    public async Task DeleteTodoItemAsync(int userId, int todoItemtId)
    {
        // Get todlist item model
        TodoItem todo = await _repository.GetByIdAsync(todoItemtId);

        // Check if it is the admin or not
        bool hasRights = await _listMemberRepository.CheckMember(userId, todo.TodoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        await _repository.DeleteAsync(todoItemtId);

        // Send when todo is updated
        await _hubContext.Clients.Group("todoList"+todo.TodoListId).SendAsync("ReceiveMessage", new
        {
            Method="loadTodoItems"
        });
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

    public async Task<TodoItem> GetTodoItemAsync(int userId, int todoItemId)
    {
        var todoItem = await _repository.GetByIdAsync(todoItemId);

        bool hasRights = await _listMemberRepository.CheckMember(userId, todoItem.TodoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }

        return todoItem;
    }

    public async Task UpdateTodoItemAsync(int userId, int todoItemtId, TodoItem todoItem)
    {
        var todoItemCheck = await _repository.GetByIdAsync(todoItemtId);

        bool hasRights = await _listMemberRepository.CheckMember(userId, todoItemCheck.TodoListId);
        if( !hasRights )
        {
            throw new InvalidCredentialException("Premissions denied");
        }
        
        await _repository.UpdateAsync(todoItemtId, todoItem);

        // Send when todo is updated
        TodoItem todo = await _repository.GetByIdAsync(todoItemtId);

        await _hubContext.Clients.Group("todoList"+todo.TodoListId).SendAsync("ReceiveMessage", new
        {
            Method="loadTodoItems"
        });
    }
}
