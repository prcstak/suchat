using Chat.Api.Producer;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;


public class ChatHub : Hub
{
    private readonly IMessageProducer _producer;

    public ChatHub(IMessageProducer producer)
    {
        _producer = producer;
    }
    
    public async Task SendMessage(string username, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", username, message, DateTime.Now);
        _producer.SendMessage(new AddMessageDto(username, message));
    }
}