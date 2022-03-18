using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class AddReportsSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE uspReportSales (@EventID int, @DateInit DATE, @DateEnd DATE)
            AS
                BEGIN

            SELECT

            DAY(S.SaleDate) AS Day,
                SUM(S.TotalSale) AS TotalSale
                FROM Sale S(NOLOCK)
            WHERE S.EventId = @EventID
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
