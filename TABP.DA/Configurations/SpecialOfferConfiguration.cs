using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class SpecialOfferConfiguration : IEntityTypeConfiguration<SpecialOffer>
{
    public void Configure(EntityTypeBuilder<SpecialOffer> builder)
    {
        builder.HasKey(so => so.Id);
    }
}