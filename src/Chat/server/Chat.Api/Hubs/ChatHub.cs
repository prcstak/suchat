using Chat.Api.Producer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

[Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub : Hub
{
    private readonly IMessageProducer _producer;

    public ChatHub(IMessageProducer producer)
    {
        _producer = producer;
    }
    
    public async Task SendMessage(string userId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", userId, message);
        _producer.SendMessage(new { UserId = userId, Message = message });
    }
}