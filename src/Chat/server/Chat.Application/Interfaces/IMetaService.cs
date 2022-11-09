using MongoDB.Bson;

namespace Chat.Application.Interfaces;

public interface IMetaService
{
    Task DeleteAsync(string filename);

    Task AddAsync(
        string metaJson,
        string filename,
        Guid id);

    Task<BsonDocument> GetMeta(string filename);
}