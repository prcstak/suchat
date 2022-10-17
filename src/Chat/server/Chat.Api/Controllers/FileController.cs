using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Png;
using Directory = System.IO.Directory;

namespace Chat.Api.Controllers;

public class FileController : BaseController
{
    private readonly IAmazonS3 _amazonS3;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileController(
        IAmazonS3 amazonS3,
        IWebHostEnvironment webHostEnvironment)
    {
        _amazonS3 = amazonS3;
        _webHostEnvironment = webHostEnvironment;
    }
    
    [HttpPost]
    [Route("bucket")]
    public async Task<IActionResult> CreateBucket(string name)
    {
        var bucketRequest = new PutBucketRequest()
        {
            BucketName = name,
            UseClientRegion = true,
        };
        
        await _amazonS3.PutBucketAsync(bucketRequest);
        
        return Ok();
    }

    [HttpGet]
    [Route("buckets")]
    public async Task<IActionResult> GetAllBuckets()
    {
        var response = await _amazonS3.ListBucketsAsync();

        return Ok(response.Buckets);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(
        string bucketName,
        IFormFile file, 
        CancellationToken cancellationToken)
    {
        await using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream, cancellationToken);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = file.FileName,
            BucketName = bucketName,
            CannedACL = S3CannedACL.PublicRead
        };

        var fileTransferUtility = new TransferUtility(_amazonS3);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        var uploads = Path.Combine(_webHostEnvironment.ContentRootPath, "_cache");
        if (!Directory.Exists(uploads))
            Directory.CreateDirectory(uploads);
        
        await using (var fileStream = new FileStream(
            Path.Combine(uploads, file.FileName),
            FileMode.Create,
            FileAccess.Write))
        {
            await file.CopyToAsync(fileStream, cancellationToken);  
        };

        var meta = JpegMetadataReader.ReadMetadata(Path.Combine(uploads, file.FileName));
        
        return Ok(meta);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadObject(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey
        };
        
        using var response = await _amazonS3.GetObjectAsync(request, cancellationToken);
        var responseStream = response.ResponseStream;
        
        return File(responseStream, "application/octet-stream");
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllObjects(string bucketName)
    {
        var response = await _amazonS3.ListObjectsAsync(bucketName);

        return Ok(response.S3Objects);
    }

    [HttpGet]
    [Route("meta")]
    public async Task<IActionResult> GetObjectMeta(string bucketName, string objectKey)
    {
        var request = new GetObjectMetadataRequest()
        {
            BucketName = bucketName,
            Key = objectKey
        };

        var response = await _amazonS3.GetObjectMetadataAsync(request);

        return Ok(response);
    }
}