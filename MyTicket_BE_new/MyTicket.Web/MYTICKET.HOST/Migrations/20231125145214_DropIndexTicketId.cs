using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class DropIndexTicketId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetail",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail",
                schema: "dbo",
                table: "OrderDetail",
                column: "EventDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetail",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail",
                schema: "dbo",
                table: "OrderDetail",
                columns: new[] { "EventDetailId", "TicketId" });
        }
    }
}
