using Chat.Application.Interfaces;
using Chat.Application.Processors;
using Chat.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IFileProcessor, FileProcessor>();
        
        return services;
    }
}