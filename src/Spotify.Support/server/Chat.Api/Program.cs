using Chat.Api.Consumers;
using Chat.Api.Extensions;
using Chat.Api.Hubs;
using Chat.Api.Producer;
using Chat.Infrastructure;
using Chat.Application;
using Chat.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCache(builder.Configuration);

builder.Services.AddCQRS();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
builder.Services.AddHostedService<MediaUploadedDispatch>();

builder.Services.AddAccessSecurity(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("All");

//app.UseAuthentication();

//app.UseAuthorization();

//app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ChatHub>("/Chat");
app.Run();