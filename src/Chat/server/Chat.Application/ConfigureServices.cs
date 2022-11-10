using Chat.Application.Extensions;
using Chat.Application.Interfaces;
using Chat.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IMetaService, MetaService>();
        
        services.AddAmazon(configuration);
        
        return services;
    }
}