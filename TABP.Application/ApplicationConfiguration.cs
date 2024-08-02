using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.Application.Services.Implementations;
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

        services.AddSingleton<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddHostedService<SpecialOfferExpirationService>();

        return services;
    }
}