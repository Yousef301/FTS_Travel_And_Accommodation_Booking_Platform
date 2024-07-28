using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMockCustomerAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Email", "FirstName", "LastName", "ModifiedAt", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"), "Admin Address", new DateOnly(2001, 9, 22), new DateTime(2024, 7, 28, 23, 32, 50, 733, DateTimeKind.Local).AddTicks(4958), "admin@example.com", "Admin", "Admin", null, "1234567890", "Admin" },
                    { new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"), "Customer Address", new DateOnly(1996, 2, 4), new DateTime(2024, 7, 28, 23, 32, 50, 733, DateTimeKind.Local).AddTicks(4963), "customer@example.com", "Customer", "Customer", null, "123456789", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedAt", "HashedPassword", "ModifiedAt", "UserId", "Username" },
                values: new object[,]
                {
                    { new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"), new DateTime(2024, 7, 28, 23, 32, 50, 732, DateTimeKind.Local).AddTicks(1685), "customer", null, new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"), "Customer1" },
                    { new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"), new DateTime(2024, 7, 28, 23, 32, 50, 732, DateTimeKind.Local).AddTicks(1680), "admin", null, new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"), "Admin1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"));

            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"));
        }
    }
}
