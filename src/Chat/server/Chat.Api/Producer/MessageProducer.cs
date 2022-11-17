using System.Text;
using System.Text.Json;
using Chat.Domain;
using RabbitMQ.Client;

namespace Chat.Api.Producer;

public class MessageProducer : IMessageProducer
{
    private readonly IConfiguration _config;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IConfiguration config, ILogger<MessageProducer> logger)
    {
        _config = config;
        _logger = logger;
    }
    public void SendMessage<T>(T message, string queue)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Hostname"],
            Port = Convert.ToInt32(_config["RabbitMQ:Port"]),
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage = JsonSerializer.Serialize(message);
        _logger.LogInformation(jsonMessage);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body);
    }
}