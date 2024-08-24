using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddThumbnailUrlFieldToTheCitiesAndHotelsAndRemoveThumbnailBoolFromImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "RoomImages");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "HotelImages");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "CityImages");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Cities");

            migrationBuilder.AddColumn<bool>(
                name: "Thumbnail",
                table: "RoomImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thumbnail",
                table: "HotelImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thumbnail",
                table: "CityImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
