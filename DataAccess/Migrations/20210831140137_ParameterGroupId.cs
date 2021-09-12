using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ParameterGroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParameterGroupId",
                table: "Parameters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_ParameterGroupId",
                table: "Parameters",
                column: "ParameterGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_ParameterGroups_ParameterGroupId",
                table: "Parameters",
                column: "ParameterGroupId",
                principalTable: "ParameterGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_ParameterGroups_ParameterGroupId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_ParameterGroupId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ParameterGroupId",
                table: "Parameters");
        }
    }
}
