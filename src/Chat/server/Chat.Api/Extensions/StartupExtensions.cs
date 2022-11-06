using System.Text;
using Chat.Api.Commands.Handler;
using Chat.Api.Queries.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Api.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddAccessSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                var secretBytes = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]);
                var key = new SymmetricSecurityKey(secretBytes);

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = key,
                };
            });

        services.AddCors(options =>
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

        return services;
    }

    public static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.AddTransient<MetaQueryHandler>();
        services.AddTransient<MetaCommandHandler>();

        return services;
    }
}