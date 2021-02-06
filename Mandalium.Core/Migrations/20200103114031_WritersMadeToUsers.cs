using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class WritersMadeToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Writers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Writers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Writers",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Writers");
        }
    }
}
