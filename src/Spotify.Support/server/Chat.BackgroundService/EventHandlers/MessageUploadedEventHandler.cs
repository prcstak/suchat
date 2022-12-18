using System.Text.Json;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Chat.Common.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService.EventHandlers;

public class MessageUploadedEventHandler : BaseEventHandler<MessageUploadedEvent>
{
    private readonly IMessageService _messageService; 
    
    public MessageUploadedEventHandler(
        IConfiguration config, 
        ILogger<EventHandler> logger, IMessageService messageService) 
        : base(config, "chat", logger)
    {
        _messageService = messageService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<MessageUploadedEvent>(body);
                
                _logger.LogInformation("Message received: " + message); 
                
                await _messageService.AddAsync(
                    new AddMessageDto(message.Username, message.Body, message.IsFile, message.Room),
                    cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception.Message);
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        
        await Task.CompletedTask;
    } 
}