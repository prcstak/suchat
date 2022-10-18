using System.Text;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Chat.Api.Hubs;
using Chat.Api.Producer;
using Chat.Infrastructure;
using Chat.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddScoped<IBrokerProducer, MessageProducer>();

var awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:AccessSecret"]),

    DefaultClientConfig =
    {
        ServiceURL = builder.Configuration["AWS:ServiceUrl"]
    }
};

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        var secretBytes = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]);
        var key = new SymmetricSecurityKey(secretBytes);

        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = key,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        policyBuilder =>
        {
            policyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
        });
});

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