using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Repositories;
using TABP.Domain.Services.Interfaces;

namespace TABP.DAL;

public static class DataAccessConfiguration
{
    public static IServiceCollection AddDataAccessInfrastructure(this IServiceCollection services,
        ISecretsManagerService secretsManagerService)
    {
        services.AddDbContext(secretsManagerService)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        ISecretsManagerService secretsManagerService)
    {
        var secrets = secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_database").Result
                      ?? throw new ArgumentNullException(nameof(secretsManagerService));

        services.AddDbContext<TABPDbContext>(options => { options.UseSqlServer(secrets["TABPDb"]); });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAmenityRepository, AmenityRepository>()
            .AddScoped<IBookingDetailRepository, BookingDetailRepository>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IImageRepository<CityImage>, CityImageRepository>()
            .AddScoped<ICityRepository, CityRepository>()
            .AddScoped<ICredentialRepository, CredentialRepository>()
            .AddScoped<IImageRepository<HotelImage>, HotelImageRepository>()
            .AddScoped<IHotelRepository, HotelRepository>()
            .AddScoped<IInvoiceRepository, InvoiceRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IRoomAmenityRepository, RoomAmenityRepository>()
            .AddScoped<IImageRepository<RoomImage>, RoomImageRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<ISpecialOfferRepository, SpecialOfferRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}