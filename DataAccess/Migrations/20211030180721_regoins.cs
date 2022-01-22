using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class regoins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chalets_Neighborhoods_NeighborhoodId",
                table: "Chalets");

            migrationBuilder.DropForeignKey(
                name: "FK_Chalets_Regions_RegionId",
                table: "Chalets");

            migrationBuilder.DropForeignKey(
                name: "FK_NeighborhoodTranslations_Neighborhoods_NeighborhoodId",
                table: "NeighborhoodTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionTranslations_Regions_RegionId",
                table: "RegionTranslations");

            migrationBuilder.DropTable(
                name: "Neighborhoods");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Chalets_NeighborhoodId",
                table: "Chalets");

            migrationBuilder.DropIndex(
                name: "IX_Chalets_RegionId",
                table: "Chalets");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Chalets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Chalets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Neighborhood1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neighborhood1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Region1",
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
                    table.PrimaryKey("PK_Region1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region1_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Region1_CityId",
                table: "Region1",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_NeighborhoodTranslations_Neighborhood1_NeighborhoodId",
                table: "NeighborhoodTranslations",
                column: "NeighborhoodId",
                principalTable: "Neighborhood1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionTranslations_Region1_RegionId",
                table: "RegionTranslations",
                column: "RegionId",
                principalTable: "Region1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NeighborhoodTranslations_Neighborhood1_NeighborhoodId",
                table: "NeighborhoodTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionTranslations_Region1_RegionId",
                table: "RegionTranslations");

            migrationBuilder.DropTable(
                name: "Neighborhood1");

            migrationBuilder.DropTable(
                name: "Region1");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Chalets");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Neighborhoods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neighborhoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neighborhoods_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chalets_NeighborhoodId",
                table: "Chalets",
                column: "NeighborhoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Chalets_RegionId",
                table: "Chalets",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Neighborhoods_RegionId",
                table: "Neighborhoods",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CityId",
                table: "Regions",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chalets_Neighborhoods_NeighborhoodId",
                table: "Chalets",
                column: "NeighborhoodId",
                principalTable: "Neighborhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chalets_Regions_RegionId",
                table: "Chalets",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NeighborhoodTranslations_Neighborhoods_NeighborhoodId",
                table: "NeighborhoodTranslations",
                column: "NeighborhoodId",
                principalTable: "Neighborhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionTranslations_Regions_RegionId",
                table: "RegionTranslations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
