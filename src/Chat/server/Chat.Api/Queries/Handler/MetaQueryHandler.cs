using Chat.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using FileNotFoundException = Chat.Common.Exceptions.FileNotFoundException;

namespace Chat.Api.Queries.Handler;

public class MetaQueryHandler : IQueryHandler<GetMetaQuery, BsonDocument>
{
    private readonly IFileMetaDbContext _context;

    public MetaQueryHandler(IFileMetaDbContext context)
    {
        _context = context;
    }
    
    public async Task<BsonDocument> Handle(GetMetaQuery query)
    {
        var meta = await _context.Files
            .Find(new BsonDocument("filename", query.Filename))
            .FirstAsync();

        if (meta == null)
            throw new FileNotFoundException(query.Filename);

        return meta;
    }
}