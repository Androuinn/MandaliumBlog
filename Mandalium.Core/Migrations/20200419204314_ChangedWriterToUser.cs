using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class ChangedWriterToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogEntries_Writers_WriterId",
                table: "BlogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Writers_WriterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Writers_WriterId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Writers");

            migrationBuilder.DropIndex(
                name: "IX_Photos_WriterId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Comments_WriterId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_BlogEntries_WriterId",
                table: "BlogEntries");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "BlogEntries");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlogEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(50)", nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(300)", nullable: true),
                    Role = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogEntries_UserId",
                table: "BlogEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogEntries_Users_UserId",
                table: "BlogEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogEntries_Users_UserId",
                table: "BlogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_UserId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Photos_UserId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_BlogEntries_UserId",
                table: "BlogEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogEntries");

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "BlogEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(300)", nullable: true),
                    Role = table.Column<bool>(type: "bit", nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_WriterId",
                table: "Photos",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WriterId",
                table: "Comments",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogEntries_WriterId",
                table: "BlogEntries",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogEntries_Writers_WriterId",
                table: "BlogEntries",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Writers_WriterId",
                table: "Comments",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Writers_WriterId",
                table: "Photos",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
