using System.Text.Json;
using Chat.Cache;
using Chat.Common.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService.EventHandlers;

public class MetaUploadedEventHandler : BaseEventHandler<MetaUploadedEvent>
{
    private readonly IRedisCache _redisCache;
    private readonly MediaProducer _mediaProducer;
    
    public MetaUploadedEventHandler(
        IConfiguration configuration,
        IRedisCache redisCache, 
        MediaProducer mediaProducer,
        ILogger<EventHandler> logger) 
        : base(configuration, "meta-uploaded", logger)
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
                var meta = JsonSerializer.Deserialize<MetaUploadedEvent>(body);

                _logger.LogInformation("Meta received: " + meta); 
                _redisCache.SetDatabase(Database.Common); 
                var reqId = meta.RequestId.ToString();
                await _redisCache.IncrementAsync(reqId);
                var val = await _redisCache.GetStringAsync(reqId);
                
                _logger.LogInformation($"ReqId value in cache is [{val}]");
                
                if (val == "2")
                    _mediaProducer.SendMessage(meta.Filename, reqId, meta.Room); 
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