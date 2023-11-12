using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(2024)",
                maxLength: 2024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2024)",
                oldMaxLength: 2024);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(2024)",
                maxLength: 2024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2024)",
                oldMaxLength: 2024,
                oldNullable: true);
        }
    }
}
