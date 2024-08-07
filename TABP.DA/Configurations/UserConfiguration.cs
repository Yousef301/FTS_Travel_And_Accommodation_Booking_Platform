using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TABP.DAL.Entities;
using TABP.Domain.Enums;

namespace TABP.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasOne(u => u.Credential)
            .WithOne(c => c.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Payments)
            .WithOne(p => p.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Bookings)
            .WithOne(b => b.User)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.Role)
            .HasConversion(new EnumToStringConverter<Role>());

        builder.HasData([
            new User
            {
                Id = new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@example.com",
                PhoneNumber = "1234567890",
                Address = "Admin Address",
                BirthDate = new DateOnly(2001, 9, 22),
                Role = Role.Admin,
                CreatedAt = DateTime.Now
            },
            new User
            {
                Id = new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                FirstName = "Customer",
                LastName = "Customer",
                Email = "customer@example.com",
                PhoneNumber = "123456789",
                Address = "Customer Address",
                BirthDate = new DateOnly(1996, 2, 4),
                Role = Role.Customer,
                CreatedAt = DateTime.Now
            }
        ]);
    }
}