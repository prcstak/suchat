using Chat.Infrastructure.Common;
using MongoDB.Driver;
using Shop.Domain;

namespace Chat.Infrastructure;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly IMongoDatabase _db;
    public IMongoCollection<User> User => _db.GetCollection<User>("User");
    public IMongoCollection<Message> Message => _db.GetCollection<Message>("Message");
    
    public ApplicationDbContext(IMongoDbConfiguration config)
    {
        var client = new MongoClient(config.ConnectionString);
        _db = client.GetDatabase(config.Database);
    }
}