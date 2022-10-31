using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Chat.Application.Interfaces;
using Chat.Common.Dto;
using Microsoft.AspNetCore.Http;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using S3Bucket = Amazon.S3.Model.S3Bucket;
using S3Object = Amazon.S3.Model.S3Object;

namespace Chat.Application.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _amazonS3;

    public FileService(
        IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task CreateBucketAsync(string name)
    {
        var bucketRequest = new PutBucketRequest()
        {
            BucketName = name,
            UseClientRegion = true,
        };
        
        await _amazonS3.PutBucketAsync(bucketRequest);
    }

    public async Task<List<S3Bucket>> GetAllBucketsAsync()
    {
        var response = await _amazonS3.ListBucketsAsync();

        return response.Buckets;
    }

    public async Task<MemoryStream> UploadFileAsync(
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

        return newMemoryStream;
    }

    public async Task<GetObjectResponse> DownloadObjectAsync(string bucketName,
        string objectKey,
        CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey
        };
        
        var response = await _amazonS3.GetObjectAsync(request, cancellationToken);

        return response;
    }

    public async Task<List<S3Object>> GetAllObjectFromBucketAsync(string bucketName)
    {
        var response = await _amazonS3.ListObjectsAsync(bucketName);

        return response.S3Objects;
    }
}