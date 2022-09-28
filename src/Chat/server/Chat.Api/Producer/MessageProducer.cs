using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Chat.Api.Producer;

public class MessageProducer : IMessageProducer
{
    public void SendMessage<T>(T message)
    {
        // REFACTOR: apply external config
        var factory = new ConnectionFactory 
        {         
            HostName = "localhost",
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "application.chat", body: body);
    }
}