using StackExchange.Redis;

namespace Chat.Cache;

public interface IRedisCache
{
    void SetDatabase(Database database);
    
    Task SetStringAsync(string key, string value);
    Task<string> GetStringAsync(string key);
}