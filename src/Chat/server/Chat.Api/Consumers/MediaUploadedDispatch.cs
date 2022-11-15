using System.Text.Json;
using Amazon.Runtime.Internal.Util;
using Chat.Api.Hubs;
using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Chat.Common.Dto;
using Chat.Common.Events;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.Api.Consumers;

public class MediaUploadedDispatch : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string MessageQueueName = "media-uploaded";
    private readonly IConfiguration _config;
    private readonly IRedisCache _redisCache;
    private readonly IFileService _fileService;
    private readonly IMetaService _metaService;
    private readonly IHubContext<ChatHub> _chatContext;
    private readonly IMessageProducer _messageProducer;
    private readonly ILogger<MediaUploadedDispatch> _logger;

    public MediaUploadedDispatch(
        IConfiguration config,
        IRedisCache redisCache, 
        IFileService fileService, 
        IMetaService metaService, 
        IHubContext<ChatHub> chatContext, 
        IMessageProducer messageProducer,
        ILogger<MediaUploadedDispatch> logger)
    {
        _config = config;
        _redisCache = redisCache;
        _fileService = fileService;
        _metaService = metaService;
        _chatContext = chatContext;
        _messageProducer = messageProducer;
        _logger = logger;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
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

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var mediaUploadedEvent = JsonSerializer.Deserialize<MediaUploadedEvent>(body);
                
                _logger.LogInformation("Media uploaded received: " + mediaUploadedEvent);

                _redisCache.SetDatabase(Database.Meta);
                var meta = await _redisCache.GetStringAsync(mediaUploadedEvent.RequestId);
                var author = await _redisCache.GetStringAsync(mediaUploadedEvent.Filename); 
                
                _redisCache.SetDatabase(Database.File);
                var filename = await _redisCache.GetStringAsync(mediaUploadedEvent.RequestId);
                
                await _fileService.MoveToPersistent(filename, cancellationToken);
                await _metaService.AddAsync(meta, filename);

                var message = "File: " + filename; 
                
                await _chatContext.Clients.All.SendAsync("ReceiveMessage", author, message, DateTime.Now);
                _messageProducer.SendMessage(new AddMessageDto(author, message), "chat");
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception);
            }
        };
        
        _channel.BasicConsume(queue: MessageQueueName, autoAck: true, consumer: consumer);
        
        await Task.CompletedTask;
    }
}