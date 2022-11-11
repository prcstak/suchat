using System.Text;
using System.Text.Json;
using Chat.Common.Events;
using RabbitMQ.Client;

namespace Chat.BackgroundService;

public class MediaProducer 
{
    private readonly IConfiguration _config;

    public MediaProducer(IConfiguration config)
    {
        _config = config;
    }
    
    public void SendMessage(string reqId)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Hostname"],
            Port = Convert.ToInt32(_config["RabbitMQ:Port"]),
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(queue: "media-uploaded",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage = JsonSerializer.Serialize<MediaUploadedEvent>(new MediaUploadedEvent(reqId));
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: "media-uploaded",
            basicProperties: null,
            body: body);
    }
}