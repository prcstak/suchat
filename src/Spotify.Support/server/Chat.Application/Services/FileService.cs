using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using S3Bucket = Amazon.S3.Model.S3Bucket;
using S3Object = Amazon.S3.Model.S3Object;

namespace Chat.Application.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string _tempBucket;
    private readonly string _persistentBucket;
    
    public FileService(
        IAmazonS3 amazonS3,
        IConfiguration configuration)
    {
        _amazonS3 = amazonS3;
        _tempBucket = configuration["AWS:Buckets:Temp"];
        _persistentBucket = configuration["AWS:Buckets:Persistent"];
    }
    
    public async Task<List<S3Bucket>> GetAllBucketsAsync()
    {
        var response = await _amazonS3.ListBucketsAsync();

        return response.Buckets;
    }

    public async Task<MemoryStream> UploadFileAsync(
        IFormFile file,
        string filename,
        CancellationToken cancellationToken)
    {
        await using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream, cancellationToken);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = filename,
            BucketName = _tempBucket, 
            CannedACL = S3CannedACL.PublicRead,
        };

        var fileTransferUtility = new TransferUtility(_amazonS3);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        return newMemoryStream;
    }

    public async Task<GetObjectResponse> DownloadObjectAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _persistentBucket, 
            Key = objectKey
        };
        
        var response = await _amazonS3.GetObjectAsync(request, cancellationToken);

        return response;
    }

    public async Task<List<S3Object>> GetAllObjectFromBucketAsync()
    {
        var response = await _amazonS3.ListObjectsAsync(_persistentBucket);

        return response.S3Objects;
    }

    public async Task MoveToPersistent(string filename, CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _tempBucket, 
            Key = filename 
        }; 
        
        var file = await _amazonS3.GetObjectAsync(request, cancellationToken);
        await using var memStream = new MemoryStream();
        await file.ResponseStream.CopyToAsync(memStream, cancellationToken);
        memStream.Seek(0, SeekOrigin.Begin);    
        
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = memStream, 
            Key = file.Key,
            BucketName = _persistentBucket,
            CannedACL = S3CannedACL.PublicRead
        };

        var fileTransferUtility = new TransferUtility(_amazonS3);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);
    }

    public async Task<ListBucketsResponse> GetAllBuckets()
    {
        return await _amazonS3.ListBucketsAsync();
    }
}