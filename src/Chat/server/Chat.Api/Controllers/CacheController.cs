using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class CacheController : BaseController
{
    private readonly IFileService _fileService;
    private readonly IMessageProducer _messageProducer;
    private readonly IRedisCache _cache;
    
    public CacheController(
        IFileService fileService,
        IMessageProducer messageProducer, 
        IRedisCache  cache)
    {
        _fileService = fileService;
        _messageProducer = messageProducer;
        _cache = cache;
        _cache.SetDatabase(Database.File);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        string requestId,
        CancellationToken cancellationToken)
    {
        var result = await _cache.GetStringAsync(requestId);

        return Ok(result);
    }
}