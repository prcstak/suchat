using Chat.Common.Exceptions;
using Chat.Infrastructure.Interfaces;
using MongoDB.Bson;
using FileNotFoundException = Chat.Common.Exceptions.FileNotFoundException;

namespace Chat.Api.Commands.Handler;

public class MetaCommandHandler :
    ICommandHandler<DeleteMetaCommand>
{
    private readonly IFileMetaDbContext _context;

    public MetaCommandHandler(IFileMetaDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMetaCommand command)
    {
        var result = await _context.Files.DeleteOneAsync(new BsonDocument("filename", command.Filename));
        if (!result.IsAcknowledged)
            throw new FileNotFoundException(command.Filename);
    }
}