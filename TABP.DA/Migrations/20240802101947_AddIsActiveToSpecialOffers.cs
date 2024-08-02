using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToSpecialOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SpecialOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 2, 13, 19, 46, 915, DateTimeKind.Local).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 2, 13, 19, 46, 915, DateTimeKind.Local).AddTicks(6740));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 2, 13, 19, 46, 916, DateTimeKind.Local).AddTicks(8333));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 2, 13, 19, 46, 916, DateTimeKind.Local).AddTicks(8339));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SpecialOffers");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 1, 18, 21, 30, 262, DateTimeKind.Local).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 1, 18, 21, 30, 262, DateTimeKind.Local).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 1, 18, 21, 30, 264, DateTimeKind.Local).AddTicks(2218));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 1, 18, 21, 30, 264, DateTimeKind.Local).AddTicks(2224));
        }
    }
}
