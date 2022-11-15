using StackExchange.Redis;

namespace Chat.Cache;

public class RedisCache : IRedisCache
{
    private IDatabase _db;
    private readonly IConnectionMultiplexer _muxer;
    
    public void SetDatabase(Database database)
    {
        _db = database switch
        {
            Database.File => _muxer.GetDatabase(1),
            Database.Meta => _muxer.GetDatabase(2),
            Database.Common => _muxer.GetDatabase(3),
            _ => throw new ArgumentException("Not supported db")
        };
    }

    public RedisCache(IConnectionMultiplexer muxer)
    {
        _muxer = muxer;
        _db = muxer.GetDatabase((int) Database.Common);
    }

    public async Task IncrementAsync(string key)
    {
        await _db.StringIncrementAsync(key);
    }
    
    public async Task SetStringAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value);
    }

    public async Task<string> GetStringAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        
        return value.ToString();
    }
}