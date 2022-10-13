using Chat.Infrastructure.Interfaces;
using MongoDB.Driver;
using File = Chat.Domain.File;

namespace Chat.Infrastructure.Contexts;

public class FileMetaDbContext : IFileMetaDbContext
{
    public IMongoCollection<File> Files { get; }
    private readonly IMongoDatabase _db;

    public FileMetaDbContext(IMongoDbConfiguration mongoDbConfiguration)
    {
        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        _db = client.GetDatabase(mongoDbConfiguration.Database);
    }
}