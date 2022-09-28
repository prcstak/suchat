using System.Text;
using System.Text.Json;
using Chat.Application.Common.Dto;
using Chat.Domain;
using Chat.Infrastructure.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService;

public class ConsumerHostedService : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string QueueName = "application.chat";
    private ILogger<ConsumerHostedService> _logger;
    private IApplicationDbContext _context;

    public ConsumerHostedService(
        IApplicationDbContext context,
        ILogger<ConsumerHostedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        // REFACTOR: apply external config
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _channel.BasicQos(0, 1, false);
        _logger.LogInformation($"[{QueueName}] has started.");

        return base.StartAsync(cancellationToken);
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PASSED");
    
        cancellationToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                var messageModel = JsonSerializer.Deserialize<AddMessageDto>(message);
                var newMessage = new Message
                {
                    GroupId = messageModel.GroupId,
                    Body = messageModel.Body,
                    Created = DateTime.Now,
                    UserId = messageModel.UserId
                };
                _logger.LogInformation($"Processing message.");
                
                await _context.Messages.AddAsync(newMessage, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation($"#{newMessage.Id} is stored.");
            }
            catch (Exception e)
            {
                _logger.LogError(default, e, e.Message);
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        await Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        _logger.LogInformation("Consumer is stopped");
    }
}