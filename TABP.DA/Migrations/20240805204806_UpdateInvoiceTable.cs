using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 5, 23, 48, 6, 54, DateTimeKind.Local).AddTicks(1379));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 5, 23, 48, 6, 54, DateTimeKind.Local).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 5, 23, 48, 6, 55, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 5, 23, 48, 6, 55, DateTimeKind.Local).AddTicks(6416));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Invoices");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 3, 20, 42, 43, 265, DateTimeKind.Local).AddTicks(6131));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 3, 20, 42, 43, 265, DateTimeKind.Local).AddTicks(6126));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 3, 20, 42, 43, 267, DateTimeKind.Local).AddTicks(357));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 8, 3, 20, 42, 43, 267, DateTimeKind.Local).AddTicks(362));
        }
    }
}
