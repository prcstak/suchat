using Chat.Api.Extensions;
using Chat.Api.Hubs;
using Chat.Api.Producer;
using Chat.Infrastructure;
using Chat.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddScoped<IBrokerProducer, MessageProducer>();

builder.Services.AddAccessSecurity(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("All");

app.UseAuthentication();

app.UseAuthorization();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Chat");

app.Run();