using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.API.Migrations
{
    public partial class AddedWriterIdToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WriterId",
                table: "Photos",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_WriterId",
                table: "Photos",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WriterId",
                table: "Comments",
                column: "WriterId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Writers_WriterId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Writers_WriterId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_WriterId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Comments_WriterId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "WriterId",
                table: "Photos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
