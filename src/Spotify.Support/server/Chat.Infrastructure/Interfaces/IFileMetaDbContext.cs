using Chat.Domain;
using MongoDB.Driver;

namespace Chat.Infrastructure.Interfaces;

public interface IFileMetaDbContext
{
    IMongoCollection<Meta> Files { get; }
}