using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterEntityOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isTransfer",
                schema: "dbo",
                table: "OrderDetail",
                newName: "IsTransfer");

            migrationBuilder.RenameColumn(
                name: "isExchange",
                schema: "dbo",
                table: "OrderDetail",
                newName: "IsExchange");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "TicketEvent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "dbo",
                table: "Ticket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "Ticket",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TicketEventId",
                schema: "dbo",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransferStatus",
                schema: "dbo",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                schema: "dbo",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_OrderDetail_Id",
                schema: "dbo",
                table: "Ticket",
                column: "Id",
                principalSchema: "dbo",
                principalTable: "OrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketEvent_OrderDetail_Id",
                schema: "dbo",
                table: "TicketEvent",
                column: "Id",
                principalSchema: "dbo",
                principalTable: "OrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_OrderDetail_Id",
                schema: "dbo",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketEvent_OrderDetail_Id",
                schema: "dbo",
                table: "TicketEvent");

            migrationBuilder.DropColumn(
                name: "TicketEventId",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TransferStatus",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Total",
                schema: "dbo",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "IsTransfer",
                schema: "dbo",
                table: "OrderDetail",
                newName: "isTransfer");

            migrationBuilder.RenameColumn(
                name: "IsExchange",
                schema: "dbo",
                table: "OrderDetail",
                newName: "isExchange");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "TicketEvent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                schema: "dbo",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "Ticket",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
