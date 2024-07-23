using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

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
    }
}