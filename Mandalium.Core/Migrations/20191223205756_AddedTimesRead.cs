using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations { 
    public partial class AddedTimesRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimesRead",
                table: "BlogEntries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesRead",
                table: "BlogEntries");
        }
    }
}
