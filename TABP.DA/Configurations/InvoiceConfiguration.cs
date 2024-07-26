using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(i => i.TotalPrice)
            .HasPrecision(10, 2);
    }
}