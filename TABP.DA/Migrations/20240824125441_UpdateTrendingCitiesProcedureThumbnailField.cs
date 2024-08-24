using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrendingCitiesProcedureThumbnailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS SP_GetTrendingCities");
            migrationBuilder.Sql(
                @"CREATE PROCEDURE SP_GetTrendingCities
                  AS
                  BEGIN
                      WITH CityBookingCounts AS (
                          SELECT 
                              c.Id,
                              c.Name AS CityName,
                              c.ThumbnailUrl AS ThumbnailUrl,
                              COUNT(b.Id) AS BookingCount
                          FROM 
                              bookings b
                          JOIN 
                              hotels h ON b.HotelId = h.Id
                          JOIN 
                              cities c ON h.CityId = c.Id
                          GROUP BY 
                              c.Id, c.Name, c.ThumbnailUrl
                      )
                      SELECT 
                          TOP 5
                          c.Id,
                          c.CityName,
                          c.ThumbnailUrl
                      FROM 
                          CityBookingCounts c
                      ORDER BY 
                          c.BookingCount DESC;
                  END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP PROCEDURE SP_GetTrendingCities");
        }
    }
}
