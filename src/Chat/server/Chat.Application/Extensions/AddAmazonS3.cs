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
        // var awsOptions = new AWSOptions
        // {
        //     Credentials = new BasicAWSCredentials(
        //         configuration["AWS:AccessKey"],
        //         configuration["AWS:AccessSecret"]),
        //
        //     DefaultClientConfig =
        //     {
        //         ServiceURL = configuration["AWS:ServiceUrl"],
        //     },
        //     DefaultConfigurationMode = DefaultConfigurationMode.Auto
        // };
        //
        // services.AddDefaultAWSOptions(awsOptions);
        // services.AddAWSService<IAmazonS3>();

        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USWest1,
                ForcePathStyle = true,
                ServiceURL = configuration["AWS:ServiceURL"]
            };

            return new AmazonS3Client(configuration["AWS:AccessKey"], configuration["AWS:AccessSecret"], config);
        });

        return services;
    }
}