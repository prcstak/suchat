using Chat.Domain;
using Chat.Infrastructure.Configurations;
using Chat.Infrastructure.Contexts;
using Chat.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure;

public static class ConfigureServices 
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetService<ApplicationDbContext>()!);
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddSingleton<IMongoDbConfiguration, MongoDbConfiguration>();

        services.AddScoped<IFileMetaDbContext, FileMetaDbContext>();
        
        return services;
    }
}