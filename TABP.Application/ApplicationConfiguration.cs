using System.Reflection;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.Application.Services.Implementations;
using TABP.Application.Services.Implementations.AWS;
using TABP.Application.Services.Interfaces;
using TABP.DAL;

namespace TABP.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDALInfrastructure(configuration);

        services.AddSingleton<ITokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<IPasswordService, BCryptPasswordService>();
        services.AddScoped<IRoomBookingService, RoomBookingService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IImageService, S3ImageService>();

        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        services.AddHostedService<SpecialOfferExpirationService>();

        return services;
    }
}