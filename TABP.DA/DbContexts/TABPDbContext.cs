using Dapper;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Models.Procedures;

namespace TABP.DAL.DbContexts;

public class TABPDbContext : DbContext
{
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<CityImage> CityImages { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<RoomImage> RoomImages { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomAmenity> RoomAmenities { get; set; }
    public DbSet<SpecialOffer> SpecialOffers { get; set; }
    public DbSet<User> Users { get; set; }

    public TABPDbContext(DbContextOptions<TABPDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public async Task<IEnumerable<TrendingCities>> GetTrendingCitiesAsync()
    {
        return await Database.GetDbConnection()
            .QueryAsync<TrendingCities>("EXEC SP_GetTrendingCities");
    }
}