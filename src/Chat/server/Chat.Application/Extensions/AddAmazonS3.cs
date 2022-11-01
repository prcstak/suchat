using System.Reflection;
using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Extensions;

public static class AmazonExtensions
{
    public static IServiceCollection AddAmazon(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var s3Config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USWest1,
            ForcePathStyle = true,
            ServiceURL = configuration["AWS:ServiceURL"]
        };
        
        services.AddSingleton<IAmazonS3>(_ 
            => GetS3Client(configuration, s3Config));
        
        AddDefaultFilesBucket(configuration, s3Config);

        return services;
    }

    private static async void AddDefaultFilesBucket(IConfiguration configuration, AmazonS3Config s3Config)
    {
        if (Assembly.GetCallingAssembly().GetName().Name == "Chat.Application")
        {
            var s3Client = GetS3Client(configuration, s3Config);
            await s3Client.PutBucketAsync(configuration["AWS:Bucket"]);   
        }
    }

    private static AmazonS3Client GetS3Client(IConfiguration configuration, AmazonS3Config s3Config) 
        => new(configuration["AWS:AccessKey"], configuration["AWS:AccessSecret"], s3Config);
}