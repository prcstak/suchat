using System.Text.Json;
using Chat.Common.Exceptions;
using Chat.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using FileNotFoundException = Chat.Common.Exceptions.FileNotFoundException;

namespace Chat.Application.Services;

public class MetaService
{
    private readonly IFileMetaDbContext _context;

    public MetaService(IFileMetaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        IReadOnlyCollection<MetadataExtractor.Directory> meta,
        string filename)
    {
        var serializedMeta = JsonSerializer.Serialize(meta);
        var metaDocument = new BsonDocument
        {
            { "id", Guid.NewGuid() },
            { "filename", filename },
            { "meta", serializedMeta }
        };
        
        await _context.Files.InsertOneAsync(metaDocument);
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