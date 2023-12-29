using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderInfoNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                schema: "dbo",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                schema: "dbo",
                table: "Notification",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                schema: "dbo",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "dbo",
                table: "Notification");
        }
    }
}
