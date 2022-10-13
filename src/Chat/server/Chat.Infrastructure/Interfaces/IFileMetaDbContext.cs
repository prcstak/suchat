using MongoDB.Driver;
using File = Chat.Domain.File;

namespace Chat.Infrastructure.Interfaces;

public interface IFileMetaDbContext
{
    IMongoCollection<File> Files { get; }
}