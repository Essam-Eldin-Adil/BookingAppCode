using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class PropertyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Neighborhoods_Cities_CityId",
                table: "Neighborhoods");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Neighborhoods",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Neighborhoods_CityId",
                table: "Neighborhoods",
                newName: "IX_Neighborhoods_RegionId");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "Chalets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chalets_RegionId",
                table: "Chalets",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CityId",
                table: "Regions",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chalets_Regions_RegionId",
                table: "Chalets",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Neighborhoods_Regions_RegionId",
                table: "Neighborhoods",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chalets_Regions_RegionId",
                table: "Chalets");

            migrationBuilder.DropForeignKey(
                name: "FK_Neighborhoods_Regions_RegionId",
                table: "Neighborhoods");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Chalets_RegionId",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Chalets");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Neighborhoods",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Neighborhoods_RegionId",
                table: "Neighborhoods",
                newName: "IX_Neighborhoods_CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Neighborhoods_Cities_CityId",
                table: "Neighborhoods",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
