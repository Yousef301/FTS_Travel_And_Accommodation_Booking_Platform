using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL.Entities;
using TABP.Shared.Enums;

namespace TABP.DAL.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.TotalPrice)
            .HasPrecision(10, 2);

        builder.Property(i => i.PaymentStatus)
            .HasConversion(new EnumToStringConverter<PaymentStatus>());
    }
}