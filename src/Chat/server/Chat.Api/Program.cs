using Chat.Api.Commands.Handler;
using Chat.Api.Extensions;
using Chat.Api.Hubs;
using Chat.Api.Producer;
using Chat.Api.Queries.Handler;
using Chat.Infrastructure;
using Chat.Application;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddTransient<MetaQueryHandler>();
builder.Services.AddTransient<MetaCommandHandler>();

builder.Services.AddSignalR();

builder.Services.AddScoped<IMessageProducer, MessageProducer>();

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

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.Run();