using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.Hotels)
            .WithOne(h => h.City)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Images)
            .WithOne(ci => ci.City)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(c => new { c.Name, c.Country });
    }
}