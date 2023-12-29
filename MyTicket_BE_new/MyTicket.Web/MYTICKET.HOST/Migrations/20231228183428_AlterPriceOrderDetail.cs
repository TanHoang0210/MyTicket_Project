using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterPriceOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "OrderDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "OrderDetail");
        }
    }
}
