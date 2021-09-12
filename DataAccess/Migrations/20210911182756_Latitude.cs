using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Latitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longtute",
                table: "Chalets",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Latetute",
                table: "Chalets",
                newName: "Latitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Chalets",
                newName: "Longtute");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Chalets",
                newName: "Latetute");
        }
    }
}
