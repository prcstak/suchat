using System.Text.Json;
using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Chat.Common.Dto;
using Chat.Common.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService;

public class ConsumerHostedService : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string MessageQueueName = "chat";
    private ILogger<ConsumerHostedService> _logger;
    private readonly IMessageService _messageService;
    private readonly IConfiguration _config;
    private readonly IRedisCache _redisCache;
    private readonly MediaProducer _mediaProducer;
    
    public ConsumerHostedService(
        ILogger<ConsumerHostedService> logger,
        IMessageService messageService,
        IConfiguration config,
        IRedisCache redisCache,
        MediaProducer mediaProducer)
    {
        _logger = logger;
        _messageService = messageService;
        _config = config;
        _redisCache = redisCache;
        _mediaProducer = mediaProducer;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        _logger.LogInformation("Consumer is stopped");
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Hostname"],
            Port = Convert.ToInt32(_config["RabbitMQ:Port"]),
        };
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();


        _channel.QueueDeclare(queue: MessageQueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _logger.LogInformation($"[{MessageQueueName}] has started.");


        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var messageConsumer = CreateMessageConsumer(cancellationToken);
        var metaUploadedEventConsumer = CreateMetaUploadedEventConsumer(cancellationToken);
        var fileUploadedEventConsumer = CreateFileUploadedEventConsumer(cancellationToken);

        _channel.BasicConsume(queue: MessageQueueName, autoAck: true, consumer: messageConsumer);
        _channel.BasicConsume(queue: "metaUploaded", autoAck: true, consumer: metaUploadedEventConsumer);
        _channel.BasicConsume(queue: "fileUploaded", autoAck: true, consumer: fileUploadedEventConsumer);

        await Task.CompletedTask;
    }

    private IBasicConsumer CreateMessageConsumer(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<AddMessageDto>(body);
                await _messageService.AddAsync(
                    new AddMessageDto(message.Username, message.Body),
                    cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception.Message);
            }
        };
        return consumer;
    }
    
    private IBasicConsumer CreateMetaUploadedEventConsumer(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var meta = JsonSerializer.Deserialize<MetaUploadedEvent>(body);

                var reqId = meta.RequestId.ToString();
                
                await _redisCache.IncrementAsync(reqId);
                var val = await _redisCache.GetStringAsync(reqId);
                if (val == "2")
                   _mediaProducer.SendMessage(meta.Filename, reqId); 
                    
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception.Message);
            }
        };
        
        return consumer;
    }
    
    private IBasicConsumer CreateFileUploadedEventConsumer(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var file = JsonSerializer.Deserialize<FileUploadedEvent>(body);
                
                var reqId = file.RequestId.ToString();
                await _redisCache.IncrementAsync(reqId);
                var val = await _redisCache.GetStringAsync(reqId);
                if (val == "2")
                    _mediaProducer.SendMessage(file.Filename, reqId); 
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception.Message);
            }
        };
        
        return consumer;
    }
}