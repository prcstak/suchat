using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Chat.Common.Events;

namespace Chat.Api.Commands.Handler;

public class MetaCommandHandler :
    ICommandHandler<SaveMetaCommand>
{
    private readonly IMetaService _fileService;
    private readonly IRedisCache _cache;
    private readonly IMessageProducer _producer;

    public MetaCommandHandler(
        IMetaService fileService, 
        IRedisCache cache,
        IMessageProducer producer)
    {
        _fileService = fileService;
        _cache = cache;
        _producer = producer;
        _cache.SetDatabase(Database.Meta);
    }
    
    public async Task Handle(SaveMetaCommand command)
    {
        await _cache.SetStringAsync(command.RequestId.ToString(), command.MetaJson);
        await _cache.SetStringAsync(command.Filename, command.Author);
        
        await _fileService.AddAsync(command.MetaJson, command.Filename);
        _producer.SendMessage<MetaUploadedEvent>(new MetaUploadedEvent(
            command.RequestId,
            command.Filename),
            "meta");
    }
}