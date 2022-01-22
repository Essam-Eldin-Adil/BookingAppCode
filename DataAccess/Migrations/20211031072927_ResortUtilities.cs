using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ResortUtilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResortParameterValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParameterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChaletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResortParameterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResortParameterValues_Chalets_ChaletId",
                        column: x => x.ChaletId,
                        principalTable: "Chalets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResortParameterValues_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResortParameterValues_ChaletId",
                table: "ResortParameterValues",
                column: "ChaletId");

            migrationBuilder.CreateIndex(
                name: "IX_ResortParameterValues_ParameterId",
                table: "ResortParameterValues",
                column: "ParameterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResortParameterValues");
        }
    }
}
