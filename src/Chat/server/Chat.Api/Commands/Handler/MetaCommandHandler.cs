using Chat.Application.Interfaces;
using Chat.Cache;

namespace Chat.Api.Commands.Handler;

public class MetaCommandHandler :
    ICommandHandler<SaveMetaCommand>
{
    private readonly IMetaService _fileService;
    private readonly IRedisCache _cache; 

    public MetaCommandHandler(IMetaService fileService, IRedisCache cache)
    {
        _fileService = fileService;
        _cache = cache;
        _cache.SetDatabase(Database.Meta);
    }
    
    public async Task Handle(SaveMetaCommand command)
    {
        await _cache.SetStringAsync(command.Id.ToString(), command.MetaJson);
        await _fileService.AddAsync(command.MetaJson, command.Filename);
    }
}