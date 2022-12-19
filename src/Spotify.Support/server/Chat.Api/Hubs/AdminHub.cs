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
        _rooms.Join(username, Context.ConnectionId, true);
    }

    public async Task LeaveUserRoom(string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
        await Clients.Group(username).SendAsync("OnAdminLeave", "admin has left the chat");
        _rooms.Leave(Context.ConnectionId);
    }
}