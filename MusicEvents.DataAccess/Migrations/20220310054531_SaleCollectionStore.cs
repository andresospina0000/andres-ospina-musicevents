using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class SaleCollectionStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE PROCEDURE uspSelectEventCollection(@GenreID INT, @DateInit DATE = NULL, @DateEnd DATE = NULL)
AS
BEGIN

    SELECT S.Id,
           E.DateEvent,
           G.Description                  Genre,
           E.ImageUrl,
           E.Title,
           S.OperationNumber,
           U.FirstName + ' ' + U.LastName FullName,
           S.Quantity,
           S.SaleDate,
           S.TotalSale
    FROM Sale S
             INNER JOIN AspNetUsers U ON S.UserId = U.Id
             INNER JOIN Events E ON E.Id = S.EventId
             INNER JOIN Genres G ON G.Id = E.GenreId
    WHERE E.GenreId = @GenreID
    AND (@DateInit IS NULL OR (S.SaleDate BETWEEN @DateInit AND @DateEnd))
    OPTION (RECOMPILE)
END
go

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE uspSelectEventCollection");
        }
    }
}
