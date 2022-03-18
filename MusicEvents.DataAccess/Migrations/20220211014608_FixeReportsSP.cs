using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class FixeReportsSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE uspReportSales(@GenreId int, @DateInit DATE, @DateEnd DATE)
AS
BEGIN

    SELECT DAY(S.SaleDate)  AS Day,
           SUM(S.TotalSale) AS TotalSale
    FROM Sale S (NOLOCK)
    INNER JOIN Events E (NOLOCK) on E.Id = S.EventId
    WHERE E.GenreId = @GenreId
      AND CAST(S.SaleDate AS DATE) BETWEEN @DateInit AND @DateEnd
    GROUP BY DAY(S.SaleDate)

END
GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE uspReportSales");
        }
    }
}
