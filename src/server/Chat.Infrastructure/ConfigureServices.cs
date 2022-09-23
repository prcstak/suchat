using Chat.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure;

public static class ConfigureServices 
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoDbConfiguration, MongoDbConfiguration>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        
        return services;
    }
}