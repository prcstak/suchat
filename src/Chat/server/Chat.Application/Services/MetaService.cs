using Chat.Application.Interfaces;
using Chat.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using FileNotFoundException = Chat.Common.Exceptions.FileNotFoundException;

namespace Chat.Application.Services;

public class MetaService : IMetaService
{
    private readonly IFileMetaDbContext _context;

    public MetaService(IFileMetaDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(string filename)
    {
        var result = await _context.Files.DeleteOneAsync(new BsonDocument("filename", filename));
        if (!result.IsAcknowledged)
            throw new FileNotFoundException(filename);
    }

    public async Task AddAsync(
        string metaJson,
        string filename,
        Guid id)
    {
        await _context.Files.InsertOneAsync(new BsonDocument
        {
            { "_id", id },
            { "filename", filename },
            { "meta", metaJson }
        });
    }

    public async Task<BsonDocument> GetMeta(string filename)
    {
        var meta = await _context.Files.Find(new BsonDocument
        {
            { "filename", filename },
        }).FirstAsync();

        if (meta == null)
            throw new FileNotFoundException(filename);

        return meta;
    }
}