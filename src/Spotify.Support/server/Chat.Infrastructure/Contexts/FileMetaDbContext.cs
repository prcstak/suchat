using Chat.Domain;
using Chat.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.Infrastructure.Contexts;

public class FileMetaDbContext : IFileMetaDbContext
{
    private readonly IMongoDatabase _db;
    public IMongoCollection<Meta> Files => _db.GetCollection<Meta>("Meta");

    public FileMetaDbContext(IMongoDbConfiguration mongoDbConfiguration)
    {
        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        _db = client.GetDatabase(mongoDbConfiguration.Database);
    }
}