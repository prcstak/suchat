using Chat.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.Infrastructure.Configurations;

public class MongoDbConfiguration : IMongoDbConfiguration
{
    public string Database { get; set; }
    public string Collection { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string ConnectionString => $@"mongodb://{User}:{Password}@{Host}:{Port}";

    public MongoDbConfiguration(IConfiguration config)
    {
        Database = config["MongoDB:Database"];
        Database = config["MongoDB:Collection"];
        Port = config["MongoDB:Port"];
        Host = config["MongoDB:Host"];
        User = config["MongoDB:User"];
        Password = config["MongoDB:Password"];
    }
}