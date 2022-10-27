using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
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
        var awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:AccessSecret"]),

            DefaultClientConfig =
            {
                ServiceURL = configuration["AWS:ServiceUrl"],
            }
        };

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();

        return services;
    }
}