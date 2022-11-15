using System.Text.Json;
using Chat.Cache;
using Chat.Common.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService.EventHandlers;

public class FileUploadedEventHandler : BaseEventHandler<FileUploadedEvent>
{
    private readonly IRedisCache _redisCache;
    private readonly MediaProducer _mediaProducer;
    
    public FileUploadedEventHandler(
        IConfiguration config,
        ILogger<EventHandler> logger,
        IRedisCache redisCache,
        MediaProducer mediaProducer) 
        : base(config, "file-uploaded", logger)
    {
        _redisCache = redisCache;
        _mediaProducer = mediaProducer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var file = JsonSerializer.Deserialize<FileUploadedEvent>(body);
                
                _logger.LogInformation("File received: " + file); 
                
                _redisCache.SetDatabase(Database.Common);
                var reqId = file.RequestId.ToString();
                await _redisCache.IncrementAsync(reqId);
                var val = await _redisCache.GetStringAsync(reqId);
                
                _logger.LogInformation($"ReqId value in cache is [{val}]");
                
                if (val == "2")
                    _mediaProducer.SendMessage(file.Filename, reqId); 
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