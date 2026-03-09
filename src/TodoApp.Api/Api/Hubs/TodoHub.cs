using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TodoApp.Api.Api.Hubs;

public class TodoHub : Hub
{
    public async Task JoinTodoList(int TodoListId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId, "todoList"+TodoListId
        );
    }

    public async Task LeaveTodoList(int TodoListId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId, "todoList"+TodoListId
        );
    }
}
