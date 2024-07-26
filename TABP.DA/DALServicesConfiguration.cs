using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Repositories;

namespace TABP.DAL;

public static class DALServicesConfiguration
{
    internal static IServiceCollection BuildInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TABPDbContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:TABPDb"]);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAmenityRepository, AmenityRepository>()
            .AddScoped<IBookingDetailRepository, BookingDetailRepository>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<ICityImageRepository, CityImageRepository>()
            .AddScoped<ICityRepository, CityRepository>()
            .AddScoped<ICredentialRepository, CredentialRepository>()
            .AddScoped<IHotelImageRepository, HotelImageRepository>()
            .AddScoped<IHotelRepository, HotelRepository>()
            .AddScoped<IInvoiceRepository, InvoiceRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IRoomAmenityRepository, RoomAmenityRepository>()
            .AddScoped<IRoomImageRepository, RoomImageRepository>()
            .AddScoped<IRoomRepository, RoomRepository>()
            .AddScoped<ISpecialOfferRepository, SpecialOfferRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}