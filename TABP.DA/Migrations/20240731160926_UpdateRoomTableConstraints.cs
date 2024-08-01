using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomTableConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 31, 19, 9, 26, 466, DateTimeKind.Local).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 31, 19, 9, 26, 466, DateTimeKind.Local).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9b4bcca-9d5b-4f3d-bf89-7a367becfbd2"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 31, 19, 9, 26, 467, DateTimeKind.Local).AddTicks(4183));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e7b7c08e-4c3a-41f5-9a9d-8571b2e4a5f4"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 31, 19, 9, 26, 467, DateTimeKind.Local).AddTicks(4189));

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages");

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("7e8b9d0a-cf4a-4a22-b7a5-7d8b2e9a6f0c"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 29, 12, 57, 37, 394, DateTimeKind.Local).AddTicks(6295));

            migrationBuilder.UpdateData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: new Guid("a3c9b0a8-d7e6-4d1c-bb9d-4d2c3bde7a1e"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 29, 12, 57, 37, 394, DateTimeKind.Local).AddTicks(6284));

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

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
