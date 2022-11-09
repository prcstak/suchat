﻿using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;
    private readonly IFileProcessor _fileProcessor;
    private readonly IMessageProducer _messageProducer;
    private readonly IDistributedCache _cache;

    public FileController(
        IFileService fileService,
        IFileProcessor fileProcessor,
        IMessageProducer messageProducer, 
        IDistributedCache cache)
    {
        _fileService = fileService;
        _fileProcessor = fileProcessor;
        _messageProducer = messageProducer;
        _cache = cache;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        string filename,
        Guid id,
        CancellationToken cancellationToken)
    {
        await using var fileStream = await _fileService.UploadFileAsync(file, cancellationToken);
        await _cache.SetAsync(id,)
        if (_fileProcessor.IsSupportedExtension(file.ContentType))
        { var metaData = await _fileProcessor.ExtractMetadataAsync(fileStream, file);
            _messageProducer.SendMessage(metaData); }
        
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