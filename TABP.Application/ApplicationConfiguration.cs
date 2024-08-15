using System.Reflection;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using TABP.Application.Helpers.Implementations;
using TABP.Application.Helpers.Interfaces;
using TABP.Application.Services.Implementations;
using TABP.Application.Services.Implementations.AWS;
using TABP.Application.Services.Interfaces;
using TABP.DAL;
using TABP.Domain.Services.Interfaces;

namespace TABP.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services,
        ISecretsManagerService secretsManagerService)
    {
        services.AddDataAccessInfrastructure(secretsManagerService);

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton<ITokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<IPasswordService, BCryptPasswordService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IImageService, S3ImageService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IHotelExpressions, HotelExpressions>();
        services.AddScoped<IPaymentService, StripePaymentService>();

        services.AddAWSService<IAmazonS3>();

        services.AddHostedService<SpecialOfferExpirationService>();

        var secrets = secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_payment").Result
                      ?? throw new ArgumentNullException(nameof(secretsManagerService));

        StripeConfiguration.ApiKey = secrets["StripeSecretKey"];

        return services;
    }
}