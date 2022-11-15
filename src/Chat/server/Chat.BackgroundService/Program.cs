using Chat.Api.Producer;
using Chat.Application;
using Chat.BackgroundService;
using Chat.BackgroundService.EventHandlers;
using Chat.Cache;
using Chat.Infrastructure;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<MessageProducer>();
        services.AddScoped<MediaProducer>();
        
        services.AddApplication(context.Configuration);
        services.AddInfrastructure(context.Configuration);
        
        services.AddCache(context.Configuration);

        services.AddHostedService<FileUploadedEventHandler>();
        services.AddHostedService<MetaUploadedEventHandler>();
        services.AddHostedService<MessageUploadedEventHandler>();
    })
    .Build();

await host.RunAsync();