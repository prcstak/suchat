using Microsoft.Extensions.DependencyInjection;

namespace Chat.BackgroundService;

public static class ConfigureServices
{
    public static IServiceCollection AddConsumerMq(this IServiceCollection services)
    {
        services.AddHostedService<ConsumerHostedService>();
        
        return services;
    }
}