using Chat.Domain;
using MongoDB.Driver;
using File = Chat.Domain.File;

namespace Chat.Infrastructure.Interfaces;

public interface IFileMetaDbContext
{
    IMongoCollection<File> Files { get; }
    public IMongoCollection<T> GetCollection<T>(string name);
}