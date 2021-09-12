using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class lastEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DayPrice",
                table: "Units",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HaveSimilar",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "IsOrigin",
                table: "Units",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsSimilar",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PromiseChangePrices",
                table: "Units",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Filterable",
                table: "ParameterGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChild",
                table: "ParameterGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ParameterGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "ParameterGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "Chalets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DayPrice",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "HaveSimilar",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsOrigin",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsSimilar",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "PromiseChangePrices",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Filterable",
                table: "ParameterGroups");

            migrationBuilder.DropColumn(
                name: "IsChild",
                table: "ParameterGroups");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ParameterGroups");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "ParameterGroups");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Chalets");
        }
    }
}
