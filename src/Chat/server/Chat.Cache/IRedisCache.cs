using StackExchange.Redis;

namespace Chat.Cache;

public interface IRedisCache
{
    IDatabase Database { get; set; }
    void SetDatabase(int database);
    Task SetStringAsync(string key, string value);
    Task<string> GetStringAsync(string key);
}