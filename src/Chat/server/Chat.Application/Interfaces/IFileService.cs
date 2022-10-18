using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces;

public interface IFileService
{
    Task CreateBucketAsync(string name);
    
    Task<List<S3Bucket>> GetAllBucketsAsync();

    Task UploadFileAsync(
        string bucketName,
        IFormFile file,
        CancellationToken cancellationToken);

    Task<Stream> DownloadObjectAsync(
        string bucketName,
        string objectKey,
        CancellationToken cancellationToken);

    Task<List<S3Object>> GetAllObjectFromBucketAsync(string bucketName);
}