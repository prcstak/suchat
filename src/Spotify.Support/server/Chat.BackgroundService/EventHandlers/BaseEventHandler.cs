using Chat.Common.Events;
using RabbitMQ.Client;

namespace Chat.BackgroundService.EventHandlers;

public abstract class BaseEventHandler<T> : Microsoft.Extensions.Hosting.BackgroundService 
    where T : IEvent
{
    protected IConnection _connection;
    protected IModel _channel;
    private ConnectionFactory _connectionFactory;
    protected readonly string _queueName;
    private readonly IConfiguration _config;
    protected ILogger<EventHandler> _logger; 
    
    protected BaseEventHandler(
        IConfiguration config,
        string queueName, 
        ILogger<EventHandler> logger)
    {
        _config = config;
        _queueName = queueName;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        
        _logger.LogInformation($"[{_queueName}] has stopped.");
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

        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        _logger.LogInformation($"[{_queueName}] has started.");

        return base.StartAsync(cancellationToken);
    }
}