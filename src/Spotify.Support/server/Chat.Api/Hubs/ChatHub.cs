using System.Text.Json;
using Chat.Api.Producer;
using Chat.Api.Utils.Rooms;
using Chat.Common.Events;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public class ChatHub : Hub
{
    private readonly IMessageProducer _producer;
    private readonly Rooms _rooms;
    private readonly IHubContext<AdminHub> _adminHub;

    public ChatHub(
        IMessageProducer producer, 
        Rooms rooms,
        IHubContext<AdminHub> adminHub)
    {
        _producer = producer;
        _rooms = rooms;
        _adminHub = adminHub;
    }
    
    public async Task SendMessage(string username, string message, string room)
    {
        await Clients.Groups(room).SendAsync("ReceiveMessage", username, message, DateTime.Now);
        _producer.SendMessage(new MessageUploadedEvent(username, message, false, room), "chat");
    }

    public async Task JoinRoom(string username, string connectionId, bool isAdmin)
    {
        _rooms.Join(username, Context.ConnectionId, isAdmin);
        await Groups.AddToGroupAsync(Context.ConnectionId, username);
        await _adminHub.Clients.All.SendAsync("RoomAmountChanged", JsonSerializer.Serialize(_rooms.GetAllWaiting()));
    }

    public async Task LeaveRoom()
    {
        var leftRoom = _rooms.Leave(Context.ConnectionId);
        var username = leftRoom.Value.IsAdmin ? "admin" : leftRoom.Value.Username;
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);

        if (!leftRoom.Value.IsAdmin)
        {
            _rooms.Close(leftRoom.Value.Username);
            await _adminHub.Clients.All.SendAsync("RoomAmountChanged", JsonSerializer.Serialize(_rooms.GetAllWaiting()));
        }

        await Clients
            .Groups(leftRoom.Value.Username)
            .SendAsync("UserLeft", $"{username} has disconnected from the chat");
    }

    public async Task CloseRoom(string username)
    {
        _rooms.Close(username);
        await Clients.Group(username).SendAsync("RoomClosed", $"Chat with {username} was closed");
        await _adminHub.Clients.All.SendAsync("RoomAmountChanged", JsonSerializer.Serialize(_rooms.GetAllWaiting()));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await LeaveRoom();
        
        await _adminHub.Clients.All.SendAsync("RoomAmountChanged", _rooms.GetAllWaiting());
        
        await base.OnDisconnectedAsync(exception);
    }
}