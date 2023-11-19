using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterEntityOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Ticket_TicketId",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_TicketId",
                schema: "dbo",
                table: "OrderDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_TicketId",
                schema: "dbo",
                table: "OrderDetail",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Ticket_TicketId",
                schema: "dbo",
                table: "OrderDetail",
                column: "TicketId",
                principalSchema: "dbo",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
