using System.Text.Json;
using Chat.Api.Utils.Rooms;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public class AdminHub : Hub
{
    private readonly Rooms _rooms;
    
    public AdminHub(Rooms rooms)
    {
        _rooms = rooms;
    }

    public async Task GetAllRooms()
    {
        await Clients.Caller.SendAsync("GetAllRooms", JsonSerializer.Serialize(_rooms.GetAllWaiting()));
    }

    public async Task CloseRoom(string username)
    {
        _rooms.Close(username);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
    }

    public async Task JoinUserRoom(string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, username);
        await Clients.Group(username).SendAsync("OnAdminJoin", "admin has joined the chat");
    }

    public async Task LeaveUserRoom(string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
        await Clients.Group(username).SendAsync("OnAdminLeave", "admin has left the chat");
    }

    public override async Task<Task> OnDisconnectedAsync(Exception? exception)
    {
        var leftRoom = _rooms.Leave(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");

        if (!leftRoom.Value.IsAdmin)
            _rooms.Close(leftRoom.Value.Username);

        await Clients
            .Groups(leftRoom.Value.Username)
            .SendAsync("UserLeft", $"admin has disconnected from the chat");
        
        return base.OnDisconnectedAsync(exception);
    }
}