using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IFileService _fileService;
    private readonly IFileProcessor _fileProcessor;

    public FileController(
        IFileService fileService,
        IFileProcessor fileProcessor)
    {
        _fileService = fileService;
        _fileProcessor = fileProcessor;
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
        await _fileService.UploadFileAsync(bucketName, file, cancellationToken);

        var metaData = await _fileProcessor.ExtractMetadataAsync(file);
        
        return Ok(metaData);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadObject(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken)
    {
        var file = await _fileService.DownloadObjectAsync(bucketName, objectKey, cancellationToken);
        
        return File(file, "application/octet-stream"); // todo: redo
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllObjects(string bucketName)
    {
        var objects = await _fileService.GetAllObjectFromBucketAsync(bucketName);

        return Ok(objects);
    }
}