using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYTICKET.Hostconsle.Migrations
{
    /// <inheritdoc />
    public partial class AlterEntityCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Suppiler");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "dbo",
                table: "Suppiler");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Language",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ShortName",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "dbo",
                table: "Customer",
                newName: "FirstName");

            migrationBuilder.AddColumn<int>(
                name: "Country",
                schema: "dbo",
                table: "Customer",
                type: "int",
                unicode: false,
                maxLength: 128,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "dbo",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "dbo",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Nationality",
                schema: "dbo",
                table: "Customer",
                type: "int",
                unicode: false,
                maxLength: 128,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer",
                schema: "dbo",
                table: "Customer",
                columns: new[] { "Deleted", "LastName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "dbo",
                table: "Customer",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Suppiler",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "Suppiler",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Customer",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "Customer",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                schema: "dbo",
                table: "Customer",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "FullName",
                value: "admin");

            migrationBuilder.CreateIndex(
                name: "IX_Customer",
                schema: "dbo",
                table: "Customer",
                columns: new[] { "Deleted", "FullName", "ShortName" });
        }
    }
}
