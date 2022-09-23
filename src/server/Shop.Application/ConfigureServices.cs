using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Common.Interfaces;

namespace Shop.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IUserService, UserService>();
        
        return services;
    }
}