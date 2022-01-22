using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChaletSettings");

            migrationBuilder.AddColumn<bool>(
                name: "CleanCondition",
                table: "Chalets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnterTime",
                table: "Chalets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitTime",
                table: "Chalets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "FamilyCondition",
                table: "Chalets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "InsuranceAmount",
                table: "Chalets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "InsuranceCondition",
                table: "Chalets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MoneyTransferCondition",
                table: "Chalets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherCondition",
                table: "Chalets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservationManager",
                table: "Chalets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservationPhoneNumber",
                table: "Chalets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactUss",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUss", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactUss");

            migrationBuilder.DropColumn(
                name: "CleanCondition",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "EnterTime",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "ExitTime",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "FamilyCondition",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "InsuranceAmount",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "InsuranceCondition",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "MoneyTransferCondition",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "OtherCondition",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "ReservationManager",
                table: "Chalets");

            migrationBuilder.DropColumn(
                name: "ReservationPhoneNumber",
                table: "Chalets");

            migrationBuilder.CreateTable(
                name: "ChaletSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChaletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CleanCondition = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FamilyCondition = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceAmount = table.Column<double>(type: "float", nullable: false),
                    InsuranceCondition = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MoneyTransferCondition = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    OtherCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChaletSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChaletSettings_Chalets_ChaletId",
                        column: x => x.ChaletId,
                        principalTable: "Chalets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChaletSettings_ChaletId",
                table: "ChaletSettings",
                column: "ChaletId");
        }
    }
}
