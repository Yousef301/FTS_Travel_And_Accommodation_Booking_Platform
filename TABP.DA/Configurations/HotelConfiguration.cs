using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.HasMany(h => h.Images)
            .WithOne(hi => hi.Hotel)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(h => h.Bookings)
            .WithOne(b => b.Hotel)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.Reviews)
            .WithOne(r => r.Hotel)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.Rating);
    }
}