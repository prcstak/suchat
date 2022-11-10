using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Chat.Cache;

public static class ConfigureServices
{
   public static IServiceCollection AddCache(
      this IServiceCollection services, 
      IConfiguration configuration)
   {
      var configurationOptions = new ConfigurationOptions()
      {
         EndPoints =
         {
            { configuration["Redis:Host"], int.Parse(configuration["Redis:Port"]) }
         },
         Password = configuration["Redis:Password"]
      };
      
      services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(configurationOptions));
      services.AddTransient<IRedisCache, RedisCache>(); 
      
      return services;
   } 
}