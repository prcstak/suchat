using Chat.Api.Producer;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;
    private readonly IFileProcessor _fileProcessor;
    private readonly IMessageProducer _messageProducer;

    public FileController(
        IFileService fileService,
        IFileProcessor fileProcessor,
        IMessageProducer messageProducer)
    {
        _fileService = fileService;
        _fileProcessor = fileProcessor;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    [Route("bucket")]
    public async Task<IActionResult> CreateBucket(string name)
    {
        await _fileService.CreateBucketAsync(name);
        
        return Ok();
    }

    [HttpGet]
    [Route("buckets")]
    public async Task<IActionResult> GetAllBuckets()
    {
        var buckets = await _fileService.GetAllBucketsAsync();

        return Ok(buckets);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        string bucketName,
        IFormFile file, 
        CancellationToken cancellationToken)
    {
        await using var fileStream = await _fileService.UploadFileAsync(bucketName, file, cancellationToken);

        if (_fileProcessor.IsSupportedExtension(file.ContentType))
        {
            var metaData = await _fileProcessor.ExtractMetadataAsync(fileStream, file);
            _messageProducer.SendMessage(metaData);
        }
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> DownloadObject(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken)
    {
        // memory leak? =)
        var file = await _fileService.DownloadObjectAsync(bucketName, objectKey, cancellationToken);

        return File(file.ResponseStream, file.Headers.ContentType, file.Key);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllObjects(string bucketName)
    {
        var objects = await _fileService.GetAllObjectFromBucketAsync(bucketName);

        return Ok(objects);
    }
}