using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AltelJobOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackgroundJobId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundJobId",
                schema: "dbo",
                table: "Order");
        }
    }
}
