using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class SaleProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE uspSelectEventById (@Id INT)
            AS
                BEGIN

            SELECT
            S.Id,
            E.DateEvent,
            G.Description Genre,
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
            WHERE S.Id = @Id

            END
                GO

            CREATE PROCEDURE uspSelectEventByUserId(@UserId NVARCHAR(36))
            AS
                BEGIN

            SELECT
            S.Id,
            E.DateEvent,
            G.Description Genre,
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
            WHERE U.Id = @UserId

            END
                GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE uspSelectEventById
                GO
                DROP PROCEDURE uspSelectEventByUserId");
        }
    }
}
