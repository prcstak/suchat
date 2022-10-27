using Amazon.S3.Model;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces;

public interface IFileService
{
    Task CreateBucketAsync(string name);
    
    Task<List<S3Bucket>> GetAllBucketsAsync();

    Task<MemoryStream> UploadFileAsync(
        string bucketName,
        IFormFile file,
        CancellationToken cancellationToken);

    Task<GetMetaDto> DownloadObjectAsync(string bucketName,
        string objectKey,
        CancellationToken cancellationToken);

    Task<List<S3Object>> GetAllObjectFromBucketAsync(string bucketName);
}