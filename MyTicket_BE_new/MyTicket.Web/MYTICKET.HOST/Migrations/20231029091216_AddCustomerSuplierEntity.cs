using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerSuplierEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppiler",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppiler", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer",
                schema: "dbo",
                table: "Customer",
                columns: new[] { "Deleted", "FullName", "ShortName" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppiler",
                schema: "dbo",
                table: "Suppiler",
                columns: new[] { "Deleted", "FullName", "ShortName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Suppiler",
                schema: "dbo");
        }
    }
}
