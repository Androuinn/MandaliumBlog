using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class AddedTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "BlogEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogEntries_TopicId",
                table: "BlogEntries",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogEntries_Topics_TopicId",
                table: "BlogEntries",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogEntries_Topics_TopicId",
                table: "BlogEntries");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_BlogEntries_TopicId",
                table: "BlogEntries");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "BlogEntries");
        }
    }
}
