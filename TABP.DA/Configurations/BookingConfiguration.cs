using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL.Entities;
using TABP.Domain.Enums;

namespace TABP.DAL.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany(b => b.BookingDetails)
            .WithOne(bd => bd.Booking)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Invoice)
            .WithOne(i => i.Booking)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Payment)
            .WithOne(p => p.Booking)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(b => b.BookingStatus)
            .HasConversion(new EnumToStringConverter<BookingStatus>());

        builder.Property(b => b.PaymentStatus)
            .HasConversion(new EnumToStringConverter<PaymentStatus>());

        builder.Property(b => b.PaymentMethod)
            .HasConversion(new EnumToStringConverter<PaymentMethod>());

        builder.Property(b => b.TotalPrice)
            .HasPrecision(10, 2);

        builder.HasIndex(b => b.BookingDate);
        builder.HasIndex(b => new { b.CheckInDate, b.CheckOutDate });
    }
}