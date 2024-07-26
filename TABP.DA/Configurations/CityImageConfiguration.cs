using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class CityImageConfiguration : IEntityTypeConfiguration<CityImage>
{
    public void Configure(EntityTypeBuilder<CityImage> builder)
    {
        builder.HasKey(ci => ci.Id);
    }
}