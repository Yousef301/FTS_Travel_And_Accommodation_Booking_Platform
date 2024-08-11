using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using TABP.Application;
using TABP.Web.Filters;
using TABP.Web.Services.Implementations;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationInfrastructure(configuration)
            .AddSwaggerConfigurations()
            .AddAuthenticationConfigurations(configuration)
            .AddFluentValidationConfigurations()
            .AddAutoMapperConfigurations()
            .AddApiVersioningConfigurations();

        services.AddScoped<IUserContext, UserContext>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    private static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            opts.IncludeXmlComments(xmlCommentsFullPath);

            opts.OperationFilter<SwaggerDefaultValues>();

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

    private static IServiceCollection AddAutoMapperConfigurations(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static IServiceCollection AddLoggerConfigurations(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/database_info.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        return services;
    }

    private static IServiceCollection AddApiVersioningConfigurations(this IServiceCollection services)
    {
        services.AddApiVersioning(opts =>
        {
            opts.DefaultApiVersion = new ApiVersion(1.0);
            opts.ReportApiVersions = true;
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'V";
            opts.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}