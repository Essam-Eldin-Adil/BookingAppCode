using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class paymentcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Invoices",
                newName: "RefNo");

            migrationBuilder.RenameColumn(
                name: "PaymentAmount",
                table: "Invoices",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "RefNo",
                table: "Invoices",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Invoices",
                newName: "PaymentAmount");
        }
    }
}
