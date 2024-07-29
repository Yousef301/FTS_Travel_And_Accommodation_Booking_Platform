using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TABP.DAL.Entities;

namespace TABP.DAL.Configurations;

public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Username)
            .IsUnique();

        builder.HasData([
            new Credential()
            {
                Id = new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                UserId = new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                Username = "Admin1",
                HashedPassword = "$2a$11$.0I3bFzDhORA0SMV8eNIieR1qJoGVWXkQkFbSVqqS6nuBOvBsdrwO",
                CreatedAt = DateTime.Now
            },
            new Credential()
            {
                Id = new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                UserId = new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                Username = "Customer1",
                HashedPassword = "$2a$11$tJjrJ/X9lTu5Vxc7T5Rv1uoNxG0QQa0wFhPEYEtvRPraUku1y8WNm",
                CreatedAt = DateTime.Now
            }
        ]);
    }
}