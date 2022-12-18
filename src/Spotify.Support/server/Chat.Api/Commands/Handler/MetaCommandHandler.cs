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
    private readonly ILogger<MetaCommandHandler> _logger;

    public MetaCommandHandler(
        IRedisCache cache,
        IMessageProducer producer,
        ILogger<MetaCommandHandler> logger)
    {
        _cache = cache;
        _producer = producer;
        _logger = logger;
        _cache.SetDatabase(Database.Meta);
    }
    
    public async Task Handle(SaveMetaCommand command)
    {
        await _cache.SetStringAsync(command.RequestId.ToString(), command.MetaJson);
        await _cache.SetStringAsync(command.Filename, command.Author);
        
        _logger.LogInformation("Meta was uploaded: " + command.RequestId);
        
        _producer.SendMessage<MetaUploadedEvent>(new MetaUploadedEvent(
            command.RequestId,
            command.Filename,
            command.Room),
            "meta-uploaded");
    }
}