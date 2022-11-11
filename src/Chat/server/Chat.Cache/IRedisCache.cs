using StackExchange.Redis;

namespace Chat.Cache;

public interface IRedisCache
{
    /// <summary>
    /// If not set, then common database is being used
    /// </summary>
    void SetDatabase(Database database);
    Task IncrementAsync(string key);
    Task SetStringAsync(string key, string value);
    Task<string> GetStringAsync(string key);
}