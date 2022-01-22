using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class CloseToSea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDayPrice",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CloseToSea",
                table: "Chalets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DistanceFromSea",
                table: "Chalets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDayPrice",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CloseToSea",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "DistanceFromSea",
                table: "Chalets");
        }
    }
}
