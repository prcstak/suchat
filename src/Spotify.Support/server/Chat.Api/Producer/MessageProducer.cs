using System.Text;
using System.Text.Json;
using Chat.Domain;
using RabbitMQ.Client;

namespace Chat.Api.Producer;

public class MessageProducer : IMessageProducer
{
    private readonly IConfiguration _config;

    public MessageProducer(IConfiguration config)
    {
        _config = config;
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
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body);
    }
}