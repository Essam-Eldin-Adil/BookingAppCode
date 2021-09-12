using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Perday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayName",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "DayNo",
                table: "PricePerDays");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PricePerDays",
                newName: "Wednesday");

            migrationBuilder.AddColumn<double>(
                name: "Friday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Monday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Saturday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Sunday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Thursday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Tuesday",
                table: "PricePerDays",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friday",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "PricePerDays");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "PricePerDays");

            migrationBuilder.RenameColumn(
                name: "Wednesday",
                table: "PricePerDays",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "DayName",
                table: "PricePerDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayNo",
                table: "PricePerDays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
