using System.Text.Json;
using Chat.Application.Interfaces;
using Chat.Cache;
using Chat.Common.Events;
using Chat.Domain;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.Api.Consumers;

public class MediaUploadedConsumer : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string MessageQueueName = "media-uploaded";
    private readonly IConfiguration _config;
    private readonly IRedisCache _redisCache;
    private readonly IFileService _fileService;
    private readonly IMetaService _metaService;

    public MediaUploadedConsumer(
        IConfiguration config,
        IRedisCache redisCache, 
        IFileService fileService, 
        IMetaService metaService)
    {
        _config = config;
        _redisCache = redisCache;
        _fileService = fileService;
        _metaService = metaService;
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

                _redisCache.SetDatabase(Database.Meta);
                var meta = await _redisCache.GetStringAsync(mediaUploadedEvent.ReqId);
                
                _redisCache.SetDatabase(Database.File);
                var filename = await _redisCache.GetStringAsync(mediaUploadedEvent.ReqId);
                
                await _metaService.AddAsync(meta, filename);
                await _fileService.MoveToPersistent(filename, cancellationToken);

            }
            catch (Exception exception)
            {
                
            }
        };
        
        _channel.BasicConsume(queue: MessageQueueName, autoAck: true, consumer: consumer);
        await Task.CompletedTask;
    }
}