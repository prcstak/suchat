using Microsoft.Extensions.Configuration;

namespace Chat.Infrastructure;

internal sealed class MongoDbConfiguration : IMongoDbConfiguration
{
    public string Database { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public string ConnectionString => $@"mongodb://{Host}:{Port}";
    
    public MongoDbConfiguration(IConfiguration config)
    {
        Database = config["MongoDB:Database"];
        Port = config["MongoDB:Port"];
        Host = config["MongoDB:Host"];
    }
}