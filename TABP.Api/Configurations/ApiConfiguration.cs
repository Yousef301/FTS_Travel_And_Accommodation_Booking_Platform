using System.Reflection;
using Amazon.CloudWatchLogs;
using Amazon.SecretsManager;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.AwsCloudWatch;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using TABP.Application;
using TABP.Shared.Services.Implementations;
using TABP.Shared.Services.Interfaces;
using TABP.Web.Filters;
using TABP.Web.Helpers.Implementations;
using TABP.Web.Helpers.Interfaces;

namespace TABP.Web.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSecretsManager>(sp => new AmazonSecretsManagerClient());
        services.AddSingleton<ISecretsManagerService, SecretsManagerService>();

        var serviceProvider = services.BuildServiceProvider();
        var secretManager = serviceProvider.GetRequiredService<ISecretsManagerService>();

        services.AddApplicationInfrastructure(secretManager)
            .AddSerilogConfigurations()
            .AddSwaggerConfigurations()
            .AddAuthenticationConfigurations(secretManager)
            .AddFluentValidationConfigurations()
            .AddAutoMapperConfigurations()
            .AddApiVersioningConfigurations()
            .AddRedisConfigurations(secretManager);

        services.AddScoped<IUserIdentity, UserIdentity>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsConfigurations>();

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
        ISecretsManagerService secretsManagerService)
    {
        var jwtConfigurations = secretsManagerService
                                    .GetSecretAsDictionaryAsync("dev_fts_jwt").Result ??
                                throw new ArgumentNullException(nameof(secretsManagerService));

        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfigurations["Issuer"],
                    ValidAudience = jwtConfigurations["Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Convert.FromBase64String(jwtConfigurations["SecretKey"])),
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

    private static IServiceCollection AddSerilogConfigurations(this IServiceCollection services)
    {
        var cloudWatchSinkOptions = new CloudWatchSinkOptions
        {
            LogGroupName = "fts-project-log-group",
            LogStreamNameProvider = new DefaultLogStreamProvider(),
            TextFormatter = new Serilog.Formatting.Json.JsonFormatter(),
            MinimumLogEventLevel = LogEventLevel.Information,
            BatchSizeLimit = 100,
            QueueSizeLimit = 10000,
            RetryAttempts = 5,
            Period = TimeSpan.FromSeconds(10),
            CreateLogGroup = true,
            LogGroupRetentionPolicy = LogGroupRetentionPolicy.OneMonth,
        };

        var cloudWatchClient = new AmazonCloudWatchLogsClient();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.AmazonCloudWatch(cloudWatchSinkOptions, cloudWatchClient)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }

    private static IServiceCollection AddRedisConfigurations(this IServiceCollection services,
        ISecretsManagerService secretsManagerService)
    {
        var redisConfigurations = secretsManagerService
                                      .GetSecretAsDictionaryAsync("dev_fts_redis").Result ??
                                  throw new ArgumentNullException(nameof(secretsManagerService));

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { redisConfigurations["ConnectionString"] },
                Password = redisConfigurations["RedisPassword"],
                ConnectTimeout = 10000,
                AbortOnConnectFail = false
            };
            options.InstanceName = "pluto-cache";
        });

        return services;
    }
}