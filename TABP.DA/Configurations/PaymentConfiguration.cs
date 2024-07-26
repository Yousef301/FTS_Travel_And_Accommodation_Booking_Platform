using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL.Entities;
using TABP.DAL.Enums;

namespace TABP.DAL.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.TotalPrice)
            .HasPrecision(10, 2);

        builder.Property(p => p.PaymentMethod)
            .HasConversion(new EnumToStringConverter<PaymentMethod>());

        builder.Property(p => p.PaymentStatus)
            .HasConversion(new EnumToStringConverter<PaymentStatus>());

        builder.HasIndex(p => p.PaymentDate);
    }
}