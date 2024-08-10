using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL.Entities;
using TABP.Domain.Enums;

namespace TABP.DAL.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.RoomAmenities)
            .WithOne(ra => ra.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Images)
            .WithOne(ri => ri.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.BookingDetails)
            .WithOne(bd => bd.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.SpecialOffers)
            .WithOne(so => so.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.RoomType)
            .HasConversion(new EnumToStringConverter<RoomType>());

        builder.Property(r => r.Status)
            .HasConversion(new EnumToStringConverter<RoomStatus>());

        builder.Property(r => r.Price)
            .HasPrecision(10, 2);

        builder.HasIndex(r => new
        {
            r.MaxAdults,
            r.MaxChildren
        });

        builder.HasIndex(r => r.RoomNumber)
            .IsUnique();

        builder.HasIndex(r => r.RoomType);
        builder.HasIndex(r => r.Price);
    }
}