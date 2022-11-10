using StackExchange.Redis;

namespace Chat.Cache;

public class RedisCache : IRedisCache
{
    public IDatabase Database { get; set; }

    private readonly IConnectionMultiplexer _muxer;

    public RedisCache(IConnectionMultiplexer muxer)
    {
        _muxer = muxer;
    }

    public void SetDatabase(int database)
    {
        Database = _muxer.GetDatabase(database);
    }

    public async Task SetStringAsync(string key, string value)
    {
        await Database.StringSetAsync(key, value);
    }

    public async Task<string> GetStringAsync(string key)
    {
        var value = await Database.StringGetAsync(key);

        return value.ToString();
    }
}