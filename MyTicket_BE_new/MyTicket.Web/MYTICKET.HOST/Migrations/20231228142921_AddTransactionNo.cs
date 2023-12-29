using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExchangeRefundRequest",
                schema: "dbo",
                table: "OrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TransferRefundRequest",
                schema: "dbo",
                table: "OrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransferTransactionNo",
                schema: "dbo",
                table: "OrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RefundRequest",
                schema: "dbo",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNo",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRefundRequest",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TransferRefundRequest",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TransferTransactionNo",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "RefundRequest",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TransactionNo",
                schema: "dbo",
                table: "Order");
        }
    }
}
