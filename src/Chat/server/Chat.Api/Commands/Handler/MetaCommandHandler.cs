using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Chat.Common.Events;

namespace Chat.Api.Commands.Handler;

public class MetaCommandHandler :
    ICommandHandler<SaveMetaCommand>
{
    private readonly IRedisCache _cache;
    private readonly IMessageProducer _producer;

    public MetaCommandHandler(
        IRedisCache cache,
        IMessageProducer producer)
    {
        _cache = cache;
        _producer = producer;
        _cache.SetDatabase(Database.Meta);
    }
    
    public async Task Handle(SaveMetaCommand command)
    {
        await _cache.SetStringAsync(command.RequestId.ToString(), command.MetaJson);
        await _cache.SetStringAsync(command.Filename, command.Author);
        
        _producer.SendMessage<MetaUploadedEvent>(new MetaUploadedEvent(
            command.RequestId,
            command.Filename),
            "meta-uploaded");
    }
}