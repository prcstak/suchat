using System.Text.Json;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
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
    private ILogger<ConsumerHostedService> _logger;
    private readonly IMessageService _messageService;
    private IApplicationDbContext _context;
    private readonly IConfiguration _config;

    public ConsumerHostedService(
        IApplicationDbContext context,
        ILogger<ConsumerHostedService> logger,
        IMessageService messageService,
        IConfiguration config)
    {
        _context = context;
        _logger = logger;
        _messageService = messageService;
        _config = config;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        // REFACTOR: apply external config
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

        _channel.BasicConsume(queue: MessageQueueName, autoAck: true, consumer: messageConsumer);

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
}