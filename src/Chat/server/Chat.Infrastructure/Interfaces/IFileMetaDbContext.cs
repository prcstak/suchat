using Chat.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using File = Chat.Domain.File;

namespace Chat.Infrastructure.Interfaces;

public interface IFileMetaDbContext
{
    IMongoCollection<BsonDocument> Files { get; }
    public IMongoCollection<T> GetCollection<T>(string name);
}