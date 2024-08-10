using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TABP.Application;
using TABP.Web.Services.Implementations;
using TABP.Web.Services.Interfaces;

namespace TABP.Web;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationInfrastructure(configuration)
            .AddSwaggerConfigurations()
            .AddAuthenticationConfigurations(configuration)
            .AddFluentValidationConfigurations()
            .AutoMapperConfigurations();

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }

    private static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Convert.FromBase64String(configuration["SecretKey"])),
                    ClockSkew = TimeSpan
                        .Zero
                };
            });

        return services;
    }

    private static IServiceCollection AddFluentValidationConfigurations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }

    private static IServiceCollection AutoMapperConfigurations(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}