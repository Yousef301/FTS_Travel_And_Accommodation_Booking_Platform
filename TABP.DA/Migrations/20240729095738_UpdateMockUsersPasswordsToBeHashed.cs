using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMockUsersPasswordsToBeHashed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                columns: new[] { "CreatedAt", "HashedPassword" },
                values: new object[] { new DateTime(2024, 7, 29, 12, 57, 37, 394, DateTimeKind.Local).AddTicks(6295), "$2a$11$tJjrJ/X9lTu5Vxc7T5Rv1uoNxG0QQa0wFhPEYEtvRPraUku1y8WNm" });

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                columns: new[] { "CreatedAt", "HashedPassword" },
                values: new object[] { new DateTime(2024, 7, 29, 12, 57, 37, 394, DateTimeKind.Local).AddTicks(6284), "$2a$11$.0I3bFzDhORA0SMV8eNIieR1qJoGVWXkQkFbSVqqS6nuBOvBsdrwO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 29, 12, 57, 37, 395, DateTimeKind.Local).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 29, 12, 57, 37, 395, DateTimeKind.Local).AddTicks(9180));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                columns: new[] { "CreatedAt", "HashedPassword" },
                values: new object[] { new DateTime(2024, 7, 28, 23, 32, 50, 732, DateTimeKind.Local).AddTicks(1685), "customer" });

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                columns: new[] { "CreatedAt", "HashedPassword" },
                values: new object[] { new DateTime(2024, 7, 28, 23, 32, 50, 732, DateTimeKind.Local).AddTicks(1680), "admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 28, 23, 32, 50, 733, DateTimeKind.Local).AddTicks(4958));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 28, 23, 32, 50, 733, DateTimeKind.Local).AddTicks(4963));
        }
    }
}
