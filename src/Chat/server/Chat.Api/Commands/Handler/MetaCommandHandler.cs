using System.Text;
using Chat.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.Api.Commands.Handler;

public class MetaCommandHandler :
    ICommandHandler<SaveMetaCommand>
{
    private readonly IMetaService _fileService;
    private readonly IDistributedCache _cache; 

    public MetaCommandHandler(IMetaService fileService, IDistributedCache cache)
    {
        _fileService = fileService;
        _cache = cache;
    }
    
    public async Task Handle(SaveMetaCommand command)
    {
        await _cache.SetAsync(command.Id.ToString(), Encoding.UTF8.GetBytes(command.MetaJson));
        await _fileService.AddAsync(command.MetaJson, command.Filename, command.Id);
    }
}