using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UnitsChalet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChaletId",
                table: "Units",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Units_ChaletId",
                table: "Units",
                column: "ChaletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Chalets_ChaletId",
                table: "Units",
                column: "ChaletId",
                principalTable: "Chalets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Chalets_ChaletId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_ChaletId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ChaletId",
                table: "Units");
        }
    }
}
