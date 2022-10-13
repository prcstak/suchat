using Chat.Application.Interfaces;
using Chat.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IFileService, FileService>();
        
        return services;
    }
}