using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicEvents.DataAccess.Migrations
{
    public partial class AddOperationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperationNumber",
                table: "Sale",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationNumber",
                table: "Sale");
        }
    }
}
