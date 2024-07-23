using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasMany(a => a.RoomAmenities)
            .WithOne(ra => ra.Amenity)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}