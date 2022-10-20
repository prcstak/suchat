﻿using System.Text.Json;
using Chat.Application;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Chat.Infrastructure;
using Chat.Infrastructure.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService;

public class ConsumerHostedService : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string MessageQueueName = "chat";
    private const string FileQueueName = "file";
    private ILogger<ConsumerHostedService> _logger;
    private readonly IMessageService _messageService;
    private IApplicationDbContext _context;

    public ConsumerHostedService(
        IApplicationDbContext context,
        ILogger<ConsumerHostedService> logger,
        IMessageService messageService)
    {
        _context = context;
        _logger = logger;
        _messageService = messageService;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        // REFACTOR: apply external config
        _connectionFactory = new ConnectionFactory
        {
            HostName = "http://localhost:5672/",
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();


        _channel.QueueDeclare(queue: MessageQueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _logger.LogInformation($"[{MessageQueueName}] has started.");

        _channel.QueueDeclare(queue: FileQueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _logger.LogInformation($"[{FileQueueName}] has started.");


        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var messageConsumer = CreateMessageConsumer(cancellationToken);
        var fileConsumer = CreateFileConsumer(cancellationToken);

        _channel.BasicConsume(queue: MessageQueueName, autoAck: true, consumer: messageConsumer);
        _channel.BasicConsume(queue: FileQueueName, autoAck: true, consumer: fileConsumer);

        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        _logger.LogInformation("Consumer is stopped");
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
    
    private IBasicConsumer CreateFileConsumer(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<IReadOnlyList<dynamic>>(body);
                
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Exception: " + exception.Message);
            }
        };
        return consumer;
    }
}