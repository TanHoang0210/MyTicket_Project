using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterRefund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundRequest",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AddColumn<bool>(
                name: "RefundRequest",
                schema: "dbo",
                table: "OrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundRequest",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.AddColumn<bool>(
                name: "RefundRequest",
                schema: "dbo",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
