using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.RoomAmenities)
            .WithOne(ra => ra.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Images)
            .WithOne(i => i.Room)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.BookingDetails)
            .WithOne(bd => bd.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.SpecialOffers)
            .WithOne(so => so.Room)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}