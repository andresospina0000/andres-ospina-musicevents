using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class PlaceForEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Events");
        }
    }
}
