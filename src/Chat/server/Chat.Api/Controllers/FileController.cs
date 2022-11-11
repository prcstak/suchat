using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;
    private readonly IMessageProducer _messageProducer;
    private readonly IRedisCache _cache;

    public FileController(
        IFileService fileService,
        IMessageProducer messageProducer, 
        IRedisCache  cache)
    {
        _fileService = fileService;
        _messageProducer = messageProducer;
        _cache = cache;
        _cache.SetDatabase(Database.File);
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        string filename,
        Guid requestId,
        CancellationToken cancellationToken)
    {
        await using var fileStream = await _fileService.UploadFileAsync(file, cancellationToken);
        await _cache.SetStringAsync(requestId.ToString(), filename); 
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> DownloadObject(
        string objectKey,
        CancellationToken cancellationToken)
    {
        // memory leak? =)
        var file = await _fileService.DownloadObjectAsync(objectKey, cancellationToken);

        return File(file.ResponseStream, file.Headers.ContentType, file.Key);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllObjects()
    {
        var objects = await _fileService.GetAllObjectFromBucketAsync();

        return Ok(objects);
    }
}