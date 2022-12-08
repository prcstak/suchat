using Chat.Domain;
using MongoDB.Bson;

namespace Chat.Application.Interfaces;

public interface IMetaService
{
    Task DeleteAsync(string filename);
    Task AddAsync(string metaJson, string filename);
    Task<Meta> GetMeta(string filename);
}