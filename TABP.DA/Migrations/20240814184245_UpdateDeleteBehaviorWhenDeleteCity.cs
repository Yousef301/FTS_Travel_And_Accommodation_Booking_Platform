using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorWhenDeleteCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CityImages_Cities_CityId",
                table: "CityImages",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
