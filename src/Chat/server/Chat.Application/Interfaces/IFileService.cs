using Amazon.S3.Model;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces;

public interface IFileService
{
    Task<List<S3Bucket>> GetAllBucketsAsync();

    Task<MemoryStream> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken);

    Task<GetObjectResponse> DownloadObjectAsync(
        string objectKey,
        CancellationToken cancellationToken);

    Task<List<S3Object>> GetAllObjectFromBucketAsync();

    Task MoveToPersistent(string filename, CancellationToken cancellationToken);
}