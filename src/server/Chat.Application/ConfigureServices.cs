using Chat.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IUserService, UserService>();
        
        return services;
    }
}