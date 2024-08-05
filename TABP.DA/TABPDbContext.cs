using Dapper;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Configurations;
using TABP.DAL.Entities;
using TABP.DAL.Models.Procedures;

namespace TABP.DAL;

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

    public TABPDbContext()
    {
    }

    public TABPDbContext(DbContextOptions<TABPDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=TABP;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AmenityConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new BookingDetailConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new CityImageConfiguration());
        modelBuilder.ApplyConfiguration(new RoomImageConfiguration());
        modelBuilder.ApplyConfiguration(new HotelImageConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new RoomAmenityConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new SpecialOfferConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    public async Task<IEnumerable<TrendingCities>> GetTrendingCitiesAsync()
    {
        return await Database.GetDbConnection()
            .QueryAsync<TrendingCities>("EXEC SP_GetTrendingCities");
    }
}