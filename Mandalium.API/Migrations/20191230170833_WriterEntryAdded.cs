using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.API.Migrations
{
    public partial class WriterEntryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WriterEntry",
                table: "BlogEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WriterEntry",
                table: "BlogEntries");
        }
    }
}
