using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Repositories;

namespace TABP.DAL;

public static class DALServicesConfiguration
{
    public static IServiceCollection AddDALInfrastructure(this IServiceCollection services,
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