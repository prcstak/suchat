namespace Chat.Infrastructure.Interfaces;

public interface IMongoDbConfiguration
{
    string Database { get; set; }
    string Collection { get; set; }
    string Host { get; set; }
    string Port { get; set; }
    string ConnectionString { get; }
}