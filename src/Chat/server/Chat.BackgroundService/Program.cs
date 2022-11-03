using Chat.Application;
using Chat.BackgroundService;
using Chat.Infrastructure;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddApplication(context.Configuration);
        services.AddInfrastructure(context.Configuration);
        services.AddHostedService<ConsumerHostedService>();
    })
    .Build();

await host.RunAsync();