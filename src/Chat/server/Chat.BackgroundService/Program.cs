using Chat.Application;
using Chat.BackgroundService;
using Chat.Infrastructure;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(config);
        services.AddHostedService<ConsumerHostedService>();
    })
    .Build();

await host.RunAsync();