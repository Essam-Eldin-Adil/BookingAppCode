using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AllowedPersons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowedPersons",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowed",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MoreThanAllowed",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MoreThanAllowedPrice",
                table: "Units",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedPersons",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "MaximumAllowed",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "MoreThanAllowed",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "MoreThanAllowedPrice",
                table: "Units");
        }
    }
}
