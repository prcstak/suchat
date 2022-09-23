namespace Chat.Infrastructure;

public interface IMongoDbConfiguration
{
    string Database { get; set; }
    string Host { get; set; }
    string Port { get; set; }
    string ConnectionString { get; }
}