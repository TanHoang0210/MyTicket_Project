using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDateExchangeTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeDate",
                schema: "dbo",
                table: "OrderDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferDate",
                schema: "dbo",
                table: "OrderDetail",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeDate",
                schema: "dbo",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "TransferDate",
                schema: "dbo",
                table: "OrderDetail");
        }
    }
}
